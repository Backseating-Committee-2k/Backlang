using Loyc.Syntax;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class UnionDeclaration : IParsePoint
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);

        iterator.Match(TokenType.OpenCurly);

        var members = ParsingHelpers.ParseSeperated<Statements.VariableStatementParser>(parser, TokenType.CloseCurly);

        return SyntaxTree.Union(nameToken.Text, members).WithRange(keywordToken, iterator.Current);
    }
}