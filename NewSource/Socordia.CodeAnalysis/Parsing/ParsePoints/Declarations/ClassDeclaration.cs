using Loyc.Syntax;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class ClassDeclaration : IParsePoint
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);
        var inheritances = new LNodeList();

        if (iterator.ConsumeIfMatch(TokenType.Colon))
        {
            inheritances = Expression.ParseList(parser, TokenType.OpenCurly, false);
        }

        iterator.Match(TokenType.OpenCurly);

        var members = ParsingHelpers.ParseUntil<TypeMemberDeclaration>(parser, TokenType.CloseCurly);

        return SyntaxTree.Class(nameToken, inheritances, members).WithRange(keywordToken, iterator.Prev);
    }
}