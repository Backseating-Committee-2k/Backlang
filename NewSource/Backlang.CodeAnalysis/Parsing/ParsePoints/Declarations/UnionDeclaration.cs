using Backlang.Codeanalysis.Core;
using Backlang.Codeanalysis.Parsing;
using Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;
using Loyc.Syntax;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class UnionDeclaration : IParsePoint
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var nameToken = iterator.Match(TokenType.Identifier);

        iterator.Match(TokenType.OpenCurly);

        var members = ParsingHelpers.ParseSeperated<VariableStatementParser>(parser, TokenType.CloseCurly);

        return SyntaxTree.Union(nameToken.Text, members).WithRange(keywordToken, iterator.Current);
    }
}