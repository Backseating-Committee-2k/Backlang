using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.TypeNames;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class EnumDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);
        TypeName baseType = new SimpleTypeName("i32");
        if (iterator.ConsumeIfMatch(TokenType.Colon))
        {
            baseType = TypeNameParser.Parse(parser);
        }

        iterator.Match(TokenType.OpenCurly);

        var members =
            ParsingHelpers.ParseSeperated<EnumMemberDeclarationParser, EnumMemberDeclaration>(parser,
                TokenType.CloseCurly);

        return new EnumDeclaration(nameToken.Text, baseType, members);
    }
}