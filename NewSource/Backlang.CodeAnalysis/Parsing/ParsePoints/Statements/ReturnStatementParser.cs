using Backlang.CodeAnalysis.AST;
using Backlang.CodeAnalysis.AST.Statements;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;

public sealed class ReturnStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        if (iterator.IsMatch(TokenType.Semicolon))
        {
            iterator.Match(TokenType.Semicolon);
            return new ReturnStatement(Expression.Parse(parser));
        }

        return null;
    }
}