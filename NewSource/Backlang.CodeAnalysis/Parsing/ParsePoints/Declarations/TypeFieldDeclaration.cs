using Backlang.Codeanalysis.Parsing;
using Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;
using Loyc.Syntax;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class TypeFieldDeclaration
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        iterator.Match(TokenType.Let);
        return VariableStatementParser.Parse(iterator, parser);
    }
}