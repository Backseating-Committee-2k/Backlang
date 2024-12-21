using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Expressions;

public sealed class InitializerListExpression : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var elements = new List<AstNode>();

        do
        {
            if (iterator.Current.Type == TokenType.CloseSquare)
            {
                break;
            }

            elements.Add(Expression.Parse(parser));

            if (iterator.Current.Type != TokenType.CloseSquare)
            {
                iterator.Match(TokenType.Comma);
            }
        } while (iterator.Current.Type != TokenType.CloseSquare);

        iterator.Match(TokenType.CloseSquare);

        return SyntaxTree.ArrayInstantiation(elements);
    }
}