using Loyc.Syntax;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements;

public sealed class TryStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var body = Statement.ParseOneOrBlock(parser);
        List<CatchStatement> catches = [];

        if (iterator.Current.Type != TokenType.Catch)
        {
            var range = new SourceRange(parser.Document, iterator.Current.Start, iterator.Current.Text.Length);

            parser.AddError(new LocalizableString(ErrorID.NoCatchBlock), range);
        }

        while (iterator.Current.Type == TokenType.Catch)
        {
            iterator.NextToken();

            catches.Add(ParseCatch(parser));
        }

        var finallly = new Block([]);
        if (iterator.IsMatch(TokenType.Finally))
        {
            iterator.Match(TokenType.Finally);
            finallly = Statement.ParseOneOrBlock(parser);
        }

        return SyntaxTree.Try(body, catches, finallly)
            .WithRange(keywordToken, iterator.Prev);
    }

    private static CatchStatement ParseCatch(Parser parser)
    {
        parser.Iterator.Match(TokenType.OpenParen);
        var exceptionValueName = new Identifier(parser.Iterator.Match(TokenType.Identifier).Text);
        parser.Iterator.Match(TokenType.Colon);
        var exceptionType = new Identifier(parser.Iterator.Match(TokenType.Identifier).Text);
        parser.Iterator.Match(TokenType.CloseParen);

        var body = Statement.ParseOneOrBlock(parser);

        return SyntaxTree.Catch(exceptionType, exceptionValueName, body);
    }
}