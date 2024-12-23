using System.Reflection;
using DistIL.AsmIO;
using DistIL.CodeGen.Cil;
using DistIL.IR;
using DistIL.IR.Utils;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using MethodBody = DistIL.IR.MethodBody;

namespace SocordiaC.Compilation;

public class CollectFunctionsListener(TypeDef type) : Listener<Driver, AstNode, FunctionDefinition>
{
    protected override void ListenToNode(Driver context, FunctionDefinition node)
    {
        var attrs = GetModifiers(node);
        var parameters = GetParameters(node);

        var method = type.CreateMethod(node.Signature.Name.Name,
            Utils.GetTypeFromNode(node.Signature.ReturnType, type), [..parameters], attrs);

        if (!node.Modifiers.Contains(Modifier.Extern))
        {
            method.Body = new MethodBody(method);
        }

        if (type == context.FunctionsType && method.IsStatic && method.Name == "main")
        {
            context.Compilation.Module.EntryPoint = method;
        }
    }

    private IEnumerable<ParamDef> GetParameters(FunctionDefinition node)
    {
        var result = new List<ParamDef>();

        foreach (var param in node.Signature.Parameters)
        {
            var paramDef = new ParamDef(new TypeSig(Utils.GetTypeFromNode(param.Type, type)), param.Name);
            //paramDef.DefaultValue = param.DefaultValue; //ToDo: convert default value
            result.Add(paramDef);
        }

        return result;
    }

    private MethodAttributes GetModifiers(Declaration node)
    {
        var attrs = (MethodAttributes)0;

        if (node.Parent is RootBlock)
        {
            attrs |= MethodAttributes.Static;
        }

        foreach (var modifier in node.Modifiers)
        {
            attrs |= modifier switch
            {
                Modifier.Static => MethodAttributes.Static,
                Modifier.Private => MethodAttributes.Private,
                Modifier.Protected => MethodAttributes.Family,
                Modifier.Internal => MethodAttributes.Assembly,
                Modifier.Public => MethodAttributes.Public,
                _ => throw new NotImplementedException()
            };
        }

        if (attrs == MethodAttributes.Static)
        {
            attrs |= MethodAttributes.Public;
        }

        return attrs;
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => false;
}