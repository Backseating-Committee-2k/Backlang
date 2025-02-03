using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public class UnitDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var nameToken = iterator.Match(TokenType.Identifier);

        iterator.Match(TokenType.Of);

        var type = new SimpleTypeName(iterator.Match(TokenType.Identifier).Text);

        iterator.Match(TokenType.Semicolon);

        return new UnitDeclaration(nameToken.Text, type);
    }
}