using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class TypeAliasDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        // using <expression> as <identifier>
        var keywordToken = iterator.Prev;
        var expr = Expression.Parse(parser); // because 'as' is binary so i32 as int resolves to as(i32, int)

        iterator.Match(TokenType.Semicolon);

        return new TypeAlias(expr);
    }
}