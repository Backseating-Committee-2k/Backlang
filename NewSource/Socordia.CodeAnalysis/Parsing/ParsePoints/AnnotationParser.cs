using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints;

public sealed class AnnotationParser
{
    public static Annotation Parse(TokenIterator iterator, Parser parser)
    {
        var atToken = iterator.Match(TokenType.At);

        var name = iterator.Match(TokenType.Identifier);
        var args = new List<AstNode>();

        if (iterator.IsMatch(TokenType.OpenParen))
        {
            iterator.NextToken();

            args = Expression.ParseList(parser, TokenType.CloseParen);
        }

        return new Annotation(name.Text, args);
    }

    public static bool TryParse(Parser parser, out List<Annotation> node)
    {
        var annotations = new List<Annotation>();

        while (IsAnnotation())
        {
            annotations.Add(Parse(parser.Iterator, parser));
        }

        node = annotations;

        return annotations.Count > 0;

        bool IsAnnotation() => parser.Iterator.IsMatch(TokenType.At) && parser.Iterator.Peek(1).Type == TokenType.Identifier;
    }
}