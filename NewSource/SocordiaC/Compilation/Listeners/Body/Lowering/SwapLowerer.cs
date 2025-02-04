using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;

namespace SocordiaC.Compilation.Listeners.Body.Lowering;

public class SwapLowerer : Replacer<AstNode, BinaryOperatorExpression>
{
    protected override AstNode? ReplaceNode(BinaryOperatorExpression node)
    {
        if (node.Operator != "<->") return node;

        //todo: implement a swap by introducting a new temp variable and test it
        var tmpName = Utils.GenerateIdentifier();
        var tmp = new VariableStatement(tmpName, node.Left.Type, node.Left, false);

        var left = new BinaryOperatorExpression("=", node.Left, node.Right);
        var right = new BinaryOperatorExpression("=", node.Right, new Identifier(tmpName));

        return new Block([tmp, left, right]);
    }
}