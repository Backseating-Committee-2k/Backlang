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
            var childType = baseType.CreateNestedType(child.Name,
                Utils.GetTypeModifiers(node), baseType: baseType);

            foreach (var parameter in child.Children.OfType<ParameterDeclaration>())
            {
                childType.CreateField(parameter.Name, new TypeSig(Utils.GetTypeFromNode(parameter.Type, baseType)), Utils.GetFieldModifiers(parameter));
            }

            CommonIR.GenerateCtor(childType);
        }

        Utils.EmitAnnotations(node, baseType);
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => false;
}