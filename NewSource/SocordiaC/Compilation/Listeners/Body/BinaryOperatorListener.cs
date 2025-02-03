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
        else if (node.Operator is "<->")
        {
            EmitSwap(context, node);
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
            context.Builder.CreateStore(vsi.Slot, rvalue);
        }
    }

    private void EmitSwap(BodyCompilation context, BinaryOperatorExpression node)
    {
        //ToDo: test swap operator
        var lvalue = context.Scope.GetFromNode(node.Left);
        var rvalue = context.Scope.GetFromNode(node.Right);

        if (lvalue is ScopeItem { IsMutable: false } si)
        {
            node.Left.AddError("Variable '" + si.Name + "' is not mutable");
            return;
        }

        if (rvalue is ScopeItem { IsMutable: false } si2)
        {
            node.Right.AddError("Variable '" + si2.Name + "' is not mutable");
            return;
        }

        //ToDo: add check if types are compatible

        //todo: genralize to allow swapping of fields and parameters too
        if (lvalue is VariableScopeItem vsi && rvalue is VariableScopeItem vsi2)
        {
            var temp = context.Builder.CreateAlloca(vsi.Type);
            context.Builder.CreateStore(temp, vsi.Slot);
            context.Builder.CreateStore(vsi.Slot, vsi2.Slot);
            context.Builder.CreateStore(vsi2.Slot, temp);
        }
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, BinaryOperatorExpression node)
    {
        return false;
    }
}
