using System.Reflection;
using DistIL.AsmIO;
using DistIL.CodeGen.Cil;
using DistIL.IR;
using DistIL.IR.Utils;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Declarations.DU;
using MethodBody = DistIL.IR.MethodBody;

namespace SocordiaC.Compilation;

public class CollectDUsListener : Listener<Driver, AstNode, DiscriminatedUnionDeclaration>
{
    protected override void ListenToNode(Driver context, DiscriminatedUnionDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var baseType = context.Compilation.Module.CreateType(ns, node.Name, Utils.GetTypeModifiers(node) | TypeAttributes.Abstract);

        foreach (var child in node.Children.OfType<DiscriminatedType>())
        {
            var childType = context.Compilation.Module.CreateType(ns, child.Name,
                Utils.GetTypeModifiers(node), baseType);

            foreach (var parameter in child.Children.OfType<ParameterDeclaration>())
            {
                childType.CreateField(parameter.Name, new TypeSig(Utils.GetTypeFromNode(parameter.Type, childType)), Utils.GetFieldModifiers(parameter));
            }

            GenerateCtor(childType, baseType, context);
        }
    }

    private void GenerateCtor(TypeDef childType, TypeDef baseType, Driver context)
    {
        var parameters = new List<ParamDef>();
        parameters.Add(new ParamDef(new TypeSig(childType), "this"));

        foreach (var field in childType.Fields)
        {
            parameters.Add(new ParamDef(new TypeSig(field.Type), field.Name));
        }

        var ctor = childType.CreateMethod(".ctor", new TypeSig(PrimType.Void), [.. parameters],
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        ctor.Body = new MethodBody(ctor);
        var irBuilder = new IRBuilder(ctor.Body.CreateBlock());

        foreach (var arg in ctor.Body.Args.Skip(1))
        {
            irBuilder.CreateFieldStore(childType.FindField(arg.Name), ctor.Body.Args[0], arg);
        }

        irBuilder.Emit(new ReturnInst());

        ctor.ILBody = ILGenerator.GenerateCode(ctor.Body);

    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => false;
}