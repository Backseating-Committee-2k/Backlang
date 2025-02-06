using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;
using Socordia.CodeAnalysis.AST.Statements;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation.Listeners.Body.Lowering;

public class SwapLowerer : Replacer<AstNode, BinaryOperatorExpression>
{
    protected override AstNode? ReplaceNode(BinaryOperatorExpression node)
    {
        if (node.Operator != "<->") return node;

        if (node.Parent is not Block)
        {
            node.AddError("Swap operator can only be used as statement");
            return node;
        }

        var leftId = (Identifier)node.Left;
        var rightId = (Identifier)node.Right;

        var tmpName = Utils.GenerateIdentifier();
        var tmp = new VariableStatement(tmpName, new NoTypeName(), new Identifier(leftId.Name), false);

        var left = new BinaryOperatorExpression("=", new Identifier(leftId.Name), new Identifier(rightId.Name));
        var right = new BinaryOperatorExpression("=", new Identifier(rightId.Name), new Identifier(tmpName));

        return new Block([tmp, left, right]);
    }
}