using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements;

public static class Statement
{
    public static Block ParseBlock(Parser parser)
    {
        var openCurlyToken = parser.Iterator.Match(TokenType.OpenCurly);

        var body = new List<AstNode>();
        while (!parser.Iterator.IsMatch(TokenType.CloseCurly) && !parser.Iterator.IsMatch(TokenType.EOF))
        {
            body.Add(parser.InvokeStatementParsePoint());
        }

        parser.Iterator.Match(TokenType.CloseCurly);

        return (Block)new Block(body).WithRange(openCurlyToken, parser.Iterator.Prev);
    }

    public static Block ParseOneOrBlock(Parser parser)
    {
        if (parser.Iterator.IsMatch(TokenType.OpenCurly))
        {
            return ParseBlock(parser);
        }

        var node = parser.InvokeStatementParsePoint();

        return new Block([node]).WithRange(node.Range);
    }
}