namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

/*
public class DestructorDeclaration : IParsePoint
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        iterator.Match(TokenType.OpenParen);

        var parameters = ParameterDeclarationParser.ParseList(parser);

        var code = Statements.Statement.ParseBlock(parser);

        return SyntaxTree.Destructor(parameters, code).WithRange(keywordToken, iterator.Prev);
    }
}
*/