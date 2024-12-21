using Backlang.CodeAnalysis.AST;
using Backlang.CodeAnalysis.AST.Statements;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;

public sealed class BreakStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        iterator.Match(TokenType.Semicolon);

        return new BreakStatement().WithRange(keywordToken, iterator.Prev);
    }
}