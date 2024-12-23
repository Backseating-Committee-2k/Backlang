using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Expressions;

public sealed class TypeOfExpressionParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        iterator.Match(TokenType.OpenParen);

        var type = TypeNameParser.Parse(parser);

        iterator.Match(TokenType.CloseParen);

        return SyntaxTree.TypeOfExpression(type);
    }
}