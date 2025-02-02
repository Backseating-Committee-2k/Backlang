using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;
using SocordiaC.Compilation.Scoping;
using SocordiaC.Compilation.Scoping.Items;

namespace SocordiaC.Compilation.Listeners.Body;

public class BinaryOperatorListener : Listener<BodyCompilation, AstNode, BinaryOperatorExpression>
{
    protected override void ListenToNode(BodyCompilation context, BinaryOperatorExpression node)
    {
        if (node.Operator is "=")
        {
            var lvalue = context.Scope.GetFromNode(node.Left);
            var rvalue = Utils.CreateValue(node.Right, context);

            if (lvalue is ScopeItem { IsMutable: false } si)
            {
                node.Left.AddError(si.Name + " is not mutable");
                return;
            }

            if (lvalue is VariableScopeItem vsi)
            {
                context.Builder.CreateStore(vsi.Slot, rvalue);
            }
        }
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, BinaryOperatorExpression node)
    {
        return false;
    }
}
