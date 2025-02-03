using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements;

public sealed class IfStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        // if cond {} else {}
        var keywordToken = iterator.Prev;
        var cond = Expression.Parse(parser);
        var body = Statement.ParseOneOrBlock(parser);
        var elseBlock = new Block([]);

        if (iterator.ConsumeIfMatch(TokenType.Else))
        {
            elseBlock = Statement.ParseOneOrBlock(parser);
        }

        return SyntaxTree.If(cond, body, elseBlock);
    }
}