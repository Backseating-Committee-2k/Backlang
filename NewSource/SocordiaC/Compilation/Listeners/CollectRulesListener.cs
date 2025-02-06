using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation.Listeners;

public class CollectRulesListener : Listener<Driver, AstNode, RulesDeclaration>
{
    protected override void ListenToNode(Driver context, RulesDeclaration node)
    {
        if (node.Target is UnitTypeName unitTypeName)
        {
            GenerateUnitConversionRules(unitTypeName, node, context);
        }

        base.ListenToNode(context, node);
    }

    private void GenerateUnitConversionRules(UnitTypeName typename, RulesDeclaration node, Driver context)
    {
        var ns = context.GetNamespaceOf(node);
        var targetType = context.Compilation.Module.FindType(ns, typename.Unit.Name);

        if (targetType is null)
        {
            typename.AddError("Unit not found");
        }

        if (!targetType.IsUnitType())
        {
            typename.AddError("Type is not a unit type");
        }
    }
}