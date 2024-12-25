using Socordia.CodeAnalysis.AST;
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
            inheritances = ParsingHelpers.ParseSeperated(parser, TokenType.OpenCurly, TypeNameParser.Parse, TokenType.Comma,false);
        }

        iterator.Match(TokenType.OpenCurly);

        List<AstNode> members = []; //ParsingHelpers.ParseUntil<TypeMemberDeclaration>(parser, TokenType.CloseCurly);

        iterator.Match(TokenType.CloseCurly); //remove if member parsing works

        return new AST.Declarations.InterfaceDeclaration(nameToken.Text, inheritances, members);
    }
}
