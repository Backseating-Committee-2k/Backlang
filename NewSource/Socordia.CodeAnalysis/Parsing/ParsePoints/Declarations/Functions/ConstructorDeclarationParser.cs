namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

/*
public class ConstructorDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        iterator.Match(TokenType.OpenParen);

        var parameters = ParameterDeclarationParser.ParseList(parser);

        var code = Statements.Statement.ParseBlock(parser);

        return SyntaxTree.Constructor(parameters, code)
            .WithRange(keywordToken, iterator.Prev);
    }
}
*/