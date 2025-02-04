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
            EmitAssignment(context, node);
        }
    }

    private void EmitAssignment(BodyCompilation context, BinaryOperatorExpression node)
    {
        var lvalue = context.Scope.GetFromNode(node.Left);
        var rvalue = Utils.CreateValue(node.Right, context);

        if (lvalue is ScopeItem { IsMutable: false } si)
        {
            node.Left.AddError("Variable '" + si.Name + "' is not mutable");
            return;
        }

        if (lvalue is VariableScopeItem vsi)
        {
            //Todo: add System.Runtime.CompilerServices.IsConst as modreq
            context.Builder.CreateStore(vsi.Slot, rvalue);
        }
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, BinaryOperatorExpression node) => false;
}
