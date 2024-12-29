using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class StructDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);
        var members = ParsingHelpers.ParseDeclarationMembers<TypeMemberDeclaration>(parser);

        return new StructDeclaration(nameToken.Text, null, [], members);
    }
}
