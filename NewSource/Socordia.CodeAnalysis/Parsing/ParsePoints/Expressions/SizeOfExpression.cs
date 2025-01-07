using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Expressions;

public sealed class SizeOfExpressionParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        //sizeof(i32)

        iterator.Match(TokenType.OpenParen);

        var type = TypeNameParser.Parse(parser);

        iterator.Match(TokenType.CloseParen);

        return new SizeOfExpression(type);
    }
}