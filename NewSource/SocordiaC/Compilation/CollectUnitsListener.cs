using System.Reflection;
using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public class CollectUnitsListener : Listener<Driver, AstNode, UnitDeclaration>
{
    protected override void ListenToNode(Driver context, UnitDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns,Utils.ToPascalCase(node.Name),
            TypeAttributes.Public, context.Compilation.Module.Resolver.Import(typeof(object)));

        var attrb = new CustomAttrib(context.KnownAttributes.MeasureAttributeCtor);
        type.GetCustomAttribs(false).Add(attrb);

        type.CreateField("Name", new TypeSig(PrimType.String),
            FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault, node.Name);
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => true;
}