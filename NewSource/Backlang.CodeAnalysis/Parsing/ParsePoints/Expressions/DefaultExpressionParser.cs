using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Expressions;

public sealed class DefaultExpressionParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        //default(i32)
        //default
        if (iterator.ConsumeIfMatch(TokenType.OpenParen))
        {
            var type = TypeLiteral.Parse(iterator, parser);

            iterator.Match(TokenType.CloseParen);

            return SyntaxTree.Default(type);
        }

        return SyntaxTree.Default();
    }
}