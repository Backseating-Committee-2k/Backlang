using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints;

public sealed class AnnotationParser
{
    public static Annotation Parse(TokenIterator iterator, Parser parser)
    {
        var atToken = iterator.Match(TokenType.At);

        //ToDo: allow support for annotations without arguments and parens
        var call = Expression.Parse(parser);

        return new Annotation(call).WithRange(atToken, iterator.Prev);
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