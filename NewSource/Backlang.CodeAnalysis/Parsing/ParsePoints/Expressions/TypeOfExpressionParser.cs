using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Expressions;

public sealed class TypeOfExpressionParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        iterator.Match(TokenType.OpenParen);

        var type = TypeLiteral.Parse(iterator, parser);

        iterator.Match(TokenType.CloseParen);

        return SyntaxTree.TypeOfExpression(type);
    }
}