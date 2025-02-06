using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public class EnumMemberDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        _ = AnnotationParser.TryParse(parser, out var annotations);

        var memberName = parser.ParseIdentifier();
        AstNode value = new EmptyNode();

        if (iterator.ConsumeIfMatch(TokenType.EqualsToken))
        {
            value = parser.ParsePrimary();
        }

        return new EnumMemberDeclaration(memberName, value)
            .WithAnnotations(annotations);
    }
}