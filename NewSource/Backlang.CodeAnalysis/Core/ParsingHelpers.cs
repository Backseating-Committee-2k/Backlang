using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.Codeanalysis.Core;

internal static class ParsingHelpers
{
    public static List<TNode> ParseSeperated<T, TNode>(
        Parser parser,
        TokenType terminator,
        TokenType seperator = TokenType.Comma, bool consumeTerminator = true)
        where T : IParsePoint
        where TNode : AstNode
    {
        if (parser.Iterator.IsMatch(terminator))
        {
            parser.Iterator.Match(terminator);
            return [];
        }

        var list = new List<TNode>();
        do
        {
            list.Add((TNode)T.Parse(parser.Iterator, parser));

            if (parser.Iterator.IsMatch(seperator) && parser.Iterator.Peek(1).Type == terminator)
            {
                parser.AddError(ErrorID.ForbiddenTrailingComma);
                parser.Iterator.Match(seperator);
            }
        } while (parser.Iterator.ConsumeIfMatch(seperator));

        if (consumeTerminator)
        {
            parser.Iterator.Match(terminator);
        }

        return list;
    }

    public static List<AstNode> ParseUntil<T>(Parser parser, TokenType terminator)
        where T : IParsePoint
    {
        var members = new List<AstNode>();
        while (parser.Iterator.Current.Type != terminator)
        {
            members.Add(T.Parse(parser.Iterator, parser));
        }

        parser.Iterator.Match(terminator);

        return members;
    }
}