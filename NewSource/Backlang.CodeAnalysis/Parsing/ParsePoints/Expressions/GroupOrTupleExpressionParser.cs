using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Expressions;

public sealed class GroupOrTupleExpressionParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var exprs = Expression.ParseList(parser, TokenType.CloseParen);

        if (exprs.Count == 1)
        {
            return exprs[0];
        }

        return SyntaxTree.Tuple(exprs);
    }
}