using System.Reflection;
using DistIL.AsmIO;
using DistIL.IR;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Literals;
using Socordia.CodeAnalysis.AST.Statements;
using Socordia.CodeAnalysis.AST.TypeNames;
using MethodBody = DistIL.IR.MethodBody;

namespace SocordiaC.Compilation;

public class CollectFunctionsListener() : Listener<Driver, AstNode, FunctionDefinition>
{
    private TypeDesc GetReturnType(FunctionDefinition node, TypeDef type)
    {
        return Utils.GetTypeFromNode(node.Signature.ReturnType, type);
    }

    protected override void ListenToNode(Driver context, FunctionDefinition node)
    {
        var attrs = GetModifiers(node);
        var type = context.GetFunctionType(context.GetNamespaceOf(node));
        var parameters = GetParameters(node, type, context);

        var returnType = GetReturnType(node, type);
        var method = type.CreateMethod(node.Signature.Name.Name,
            returnType, [..parameters], attrs);

        if (!node.Modifiers.Contains(Modifier.Extern))
        {
            method.Body = new MethodBody(method);
        }

        if (type.Name == "Functions" && method is { IsStatic: true, Name: "main" })
        {
            context.Compilation.Module.EntryPoint = method;
        }

        Mappings.Functions.Add(node, method);
    }

    private IEnumerable<ParamDef> GetParameters(FunctionDefinition node, TypeDef type, Driver context)
    {
        var result = new List<ParamDef>();

        foreach (var param in node.Signature.Parameters)
        {
            var attribs = GetParameterAttributes(param);
            var paramDef = new ParamDef(new TypeSig(Utils.GetTypeFromNode(param.Type, type)), param.Name, attribs)
                {
                    DefaultValue = Utils.GetLiteralValue(param.DefaultValue)
                };

            result.Add(paramDef);
        }

        return result;
    }

    private ParameterAttributes GetParameterAttributes(ParameterDeclaration node)
    {
        ParameterAttributes result = 0;

        if (node.DefaultValue != null)
        {
            result |= ParameterAttributes.HasDefault | ParameterAttributes.Optional;
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