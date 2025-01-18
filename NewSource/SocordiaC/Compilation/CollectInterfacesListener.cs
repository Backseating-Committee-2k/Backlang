using System.Reflection;
using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public class CollectInterfacesListener : Listener<Driver, AstNode, InterfaceDeclaration>
{
    protected override void ListenToNode(Driver context, InterfaceDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name,
            Utils.GetTypeModifiers(node) | TypeAttributes.Interface | TypeAttributes.Abstract,
            context.Compilation.Resolver.SysTypes.Object);

        Utils.EmitAnnotations(node, type);
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => true;
}