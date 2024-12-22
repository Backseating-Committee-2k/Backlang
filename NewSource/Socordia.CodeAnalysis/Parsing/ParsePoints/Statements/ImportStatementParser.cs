using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements;

public sealed class ImportStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        //import <identifier>
        //import <identifier>.<identifier>
        var keywordToken = iterator.Prev;
        var tree = SyntaxTree.Import(Expression.Parse(parser));

        iterator.Match(TokenType.Semicolon);

        return tree;
    }
}