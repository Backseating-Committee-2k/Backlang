using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements.Loops;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements.Loops;

public sealed class ContinueStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        iterator.Match(TokenType.Semicolon);

        return new ContinueStatement();
    }
}