using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements;

public sealed class ReturnStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        if (iterator.ConsumeIfMatch(TokenType.Semicolon))
        {
            return new ReturnStatement(new EmptyNode());
        }

        AstNode node = new ReturnStatement(Expression.Parse(parser));
        iterator.Match(TokenType.Semicolon);

        return node;
    }
}