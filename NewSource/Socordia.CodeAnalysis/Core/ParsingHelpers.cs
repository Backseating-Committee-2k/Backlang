using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.Parsing;

namespace Socordia.CodeAnalysis.Core;

internal static class ParsingHelpers
{
    public static List<T> ParseDeclarationMembers<T>(Parser parser)
        where T : AstNode
    {
        if (parser.Iterator.ConsumeIfMatch(TokenType.Semicolon))
        {
            return [];
        }

        parser.Iterator.Match(TokenType.OpenCurly);
        List<T> members = []; //ParsingHelpers.ParseUntil<TypeMemberDeclaration>(parser, TokenType.CloseCurly);

        parser.Iterator.Match(TokenType.CloseCurly); //remove if member parsing works

        return members;
    }

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
                parser.AddError("Trailing comma is forbidden");
                parser.Iterator.Match(seperator);
            }
        } while (parser.Iterator.ConsumeIfMatch(seperator));

        if (consumeTerminator)
        {
            parser.Iterator.Match(terminator);
        }

        return list;
    }

    public static List<TResult> ParseSeperated<TResult>(
        Parser parser,
        TokenType terminator,
        Func<Parser, TResult> builder,
        TokenType seperator = TokenType.Comma, bool consumeTerminator = true)
        where TResult : AstNode
    {
        if (parser.Iterator.IsMatch(terminator))
        {
            parser.Iterator.Match(terminator);
            return [];
        }

        var list = new List<TResult>();
        do
        {
            list.Add(builder(parser));

            if (parser.Iterator.IsMatch(seperator) && parser.Iterator.Peek(1).Type == terminator)
            {
                parser.AddError("Trailing comma is forbidden");
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