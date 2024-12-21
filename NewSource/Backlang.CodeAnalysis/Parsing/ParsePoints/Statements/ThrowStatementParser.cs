using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;

public class ThrowStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        if (!iterator.IsMatch(TokenType.Semicolon))
        {
            var arg = Expression.Parse(parser);
            iterator.Match(TokenType.Semicolon);

            return SyntaxTree.Throw(arg).WithRange(keywordToken, iterator.Prev);
        }

        return null;
    }
}