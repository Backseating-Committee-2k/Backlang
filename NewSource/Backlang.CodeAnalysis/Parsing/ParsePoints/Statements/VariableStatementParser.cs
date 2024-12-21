using Backlang.CodeAnalysis.AST;
using Backlang.CodeAnalysis.AST.Statements;
using Backlang.Codeanalysis.Parsing;
using Loyc.Syntax;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;

public class VariableStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var isMutable = false;
        var type = SyntaxTree.Type("", LNode.List());

        Token mutableToken = null;

        if (iterator.Current.Type == TokenType.Mutable)
        {
            isMutable = true;
            iterator.NextToken();
        }

        var nameToken = iterator.Match(TokenType.Identifier);

        if (iterator.Current.Type == TokenType.Colon)
        {
            iterator.NextToken();

            type = TypeLiteral.Parse(iterator, parser);
        }

        AstNode initilizer = null;
        if (iterator.Current.Type == TokenType.EqualsToken)
        {
            iterator.NextToken();

            initilizer = Expression.Parse(parser);
        }

        iterator.Match(TokenType.Semicolon);

        return new VariableStatement(nameToken.Text, type, initilizer, isMutable)
            .WithRange(keywordToken, iterator.Prev);
    }
}