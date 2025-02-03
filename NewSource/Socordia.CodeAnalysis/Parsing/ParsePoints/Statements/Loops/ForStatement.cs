using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements.Loops;

public sealed class ForStatement : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        //for x : i32 in 1..12
        //for x in arr
        var keywordToken = iterator.Prev;
        var varExpr = Expression.Parse(parser);

        TypeName type = null;

        if (iterator.Current.Type == TokenType.Colon)
        {
            iterator.NextToken();

            type = TypeNameParser.Parse(parser);
        }

        iterator.Match(TokenType.In);

        var collExpr = Expression.Parse(parser);
        var body = Statement.ParseOneOrBlock(parser);

        return SyntaxTree.For(varExpr, type, collExpr, body);
    }
}