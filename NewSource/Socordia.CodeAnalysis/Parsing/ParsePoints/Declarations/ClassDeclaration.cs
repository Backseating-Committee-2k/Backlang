using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class ClassDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);
        var inheritances = new List<AstNode>();

        if (iterator.ConsumeIfMatch(TokenType.Colon))
        {
            inheritances = Expression.ParseList(parser, TokenType.OpenCurly, false);
        }

        iterator.Match(TokenType.OpenCurly);

        List<AstNode> members = []; //ParsingHelpers.ParseUntil<TypeMemberDeclaration>(parser, TokenType.CloseCurly);

        return SyntaxTree.Class(nameToken, inheritances, members);
    }
}