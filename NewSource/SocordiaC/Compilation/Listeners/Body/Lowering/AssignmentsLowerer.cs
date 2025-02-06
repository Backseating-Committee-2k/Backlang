using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;

namespace SocordiaC.Compilation.Listeners.Body.Lowering;

public class AssignmentsLowerer : Replacer<AstNode, BinaryOperatorExpression>
{
    private static readonly List<string> ShortAssignmentOperators =
    [
        "+=",
        "-=",
        "/=",
        "*="
    ];

    protected override AstNode? ReplaceNode(BinaryOperatorExpression node)
    {
        if (!ShortAssignmentOperators.Contains(node.Operator))
        {
            return node;
        }

        var left = node.Left;
        var right = node.Right;
        node.Left.RemoveFromParent();
        node.Right.RemoveFromParent();

        var newOperator = node.Operator.Replace("=", "");
        return new BinaryOperatorExpression("=", left,
            new BinaryOperatorExpression(newOperator, left, right)
        );
    }
}