using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Literals;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Expressions;

public sealed class DefaultExpressionParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        //default(i32)
        //default
        if (iterator.ConsumeIfMatch(TokenType.OpenParen))
        {
            var type = TypeNameParser.Parse(parser);

            iterator.Match(TokenType.CloseParen);

            return new DefaultLiteral(type);
        }

        return new DefaultLiteral(null);
    }
}