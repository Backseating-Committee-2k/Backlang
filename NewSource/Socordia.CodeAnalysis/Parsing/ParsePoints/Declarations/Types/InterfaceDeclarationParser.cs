using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.TypeNames;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class InterfaceDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);
        var inheritances = new List<TypeName>();

        if (iterator.ConsumeIfMatch(TokenType.Colon))
        {
            inheritances = ParsingHelpers.ParseSeperated(parser, TokenType.OpenCurly, TypeNameParser.Parse,
                TokenType.Comma, false);
        }

        var members = ParsingHelpers.ParseDeclarationMembers<TypeMemberDeclaration>(parser);

        return new InterfaceDeclaration(nameToken.Text, inheritances, members);
    }
}