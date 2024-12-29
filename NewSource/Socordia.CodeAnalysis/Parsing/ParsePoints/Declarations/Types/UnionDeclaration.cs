using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class UnionDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);

        iterator.Match(TokenType.OpenCurly);

        List<AstNode> members = []; //ParsingHelpers.ParseUntil<VariableStatementParser>(parser, TokenType.CloseCurly);

        iterator.Match(TokenType.CloseCurly); //remove if member parsing works

        return new UnionDeclaration(nameToken.Text, members);
    }
}