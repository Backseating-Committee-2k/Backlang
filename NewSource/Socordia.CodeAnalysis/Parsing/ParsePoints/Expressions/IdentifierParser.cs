using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Expressions;

public sealed class IdentifierParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var nameToken = iterator.Peek(-1);
        var nameExpression = SyntaxTree.Id(nameToken.Text);

        if (iterator.Current.Type == TokenType.OpenSquare)
        {
            iterator.NextToken();

            return SyntaxTree.ArrayInstantiation(nameExpression, Expression.ParseList(parser, TokenType.CloseSquare));
        }

        if (iterator.Current.Type == TokenType.OpenParen)
        {
            iterator.NextToken();

            var arguments = Expression.ParseList(parser, TokenType.CloseParen);

            return new Call(nameExpression, arguments);
        }

        return nameExpression;
    }
}