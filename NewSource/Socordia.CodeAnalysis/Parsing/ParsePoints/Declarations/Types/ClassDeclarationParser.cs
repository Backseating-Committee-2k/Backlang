using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.TypeNames;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class ClassDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);
        var inheritances = new List<TypeName>();
        TypeName? baseType = null;

        var implementsParsed = false;
        var extendsParsed = false;

        while (iterator.IsMatch(TokenType.Implements) || iterator.IsMatch(TokenType.Extends))
            if (iterator.Current.Type == TokenType.Implements && !implementsParsed)
            {
                iterator.NextToken();
                inheritances = ParsingHelpers.ParseSeperated(parser, TokenType.OpenCurly, TypeNameParser.Parse,
                    TokenType.Comma, false);
                implementsParsed = true;
            }
            else if (iterator.Current.Type == TokenType.Extends && !extendsParsed)
            {
                iterator.NextToken();
                baseType = TypeNameParser.Parse(parser);
                extendsParsed = true;
            }

        var members = ParsingHelpers.ParseDeclarationMembers<TypeMemberDeclaration>(parser);
        return new ClassDeclaration(nameToken.Text, baseType, inheritances, members);
    }
}