using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;

public sealed class IfStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        // if cond {} else {}
        var keywordToken = iterator.Prev;
        var cond = Expression.Parse(parser);
        var body = Statement.ParseOneOrBlock(parser);
        AstNode? elseBlock = new Block([]);

        if (iterator.Current.Type == TokenType.Else)
        {
            iterator.NextToken();

            elseBlock = Statement.ParseOneOrBlock(parser);
        }

        return SyntaxTree.If(cond, body, elseBlock).WithRange(keywordToken, iterator.Prev);
    }
}