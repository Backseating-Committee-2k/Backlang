using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class ParameterDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        _ = AnnotationParser.TryParse(parser, out var annotations);

        var isOut = iterator.ConsumeIfMatch(TokenType.Out);

        var name = iterator.Match(TokenType.Identifier);

        var assertNotNull = iterator.ConsumeIfMatch(TokenType.Exclamation);

        iterator.Match(TokenType.Colon);

        var type = TypeNameParser.Parse(parser);

        AstNode? defaultValue = null;

        if (iterator.Current.Type == TokenType.EqualsToken)
        {
            iterator.NextToken();

            defaultValue = Expression.Parse(parser);
        }

        return new ParameterDeclaration(type, name.Text, defaultValue, assertNotNull, annotations, isOut);
    }

    public static List<ParameterDeclaration> ParseList(Parser parser)
    {
        return ParsingHelpers
            .ParseSeperated<ParameterDeclarationParser, ParameterDeclaration>(parser, TokenType.CloseParen);
    }
}