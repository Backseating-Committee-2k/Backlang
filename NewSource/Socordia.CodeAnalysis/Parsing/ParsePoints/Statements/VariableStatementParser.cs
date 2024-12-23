using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Statements;

public class VariableStatementParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        var isMutable = false;
        TypeName? type = null;

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

            type = TypeNameParser.Parse(parser);
        }

        AstNode initilizer = null;
        if (iterator.Current.Type == TokenType.EqualsToken)
        {
            iterator.NextToken();

            initilizer = Expression.Parse(parser);
        }

        iterator.Match(TokenType.Semicolon);

        return new VariableStatement(nameToken.Text, type, initilizer, isMutable);
    }
}