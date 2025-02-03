using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements;

public sealed class ImportStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        //import <identifier>
        //import <identifier>.<identifier>
        var keywordToken = iterator.Prev;
        var tree = new ImportStatement(Expression.Parse(parser));

        iterator.Match(TokenType.Semicolon);

        return tree;
    }
}