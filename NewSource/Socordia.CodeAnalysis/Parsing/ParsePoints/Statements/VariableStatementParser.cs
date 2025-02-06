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
        TypeName? type = NoTypeName.Instance;

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

        AstNode initializer = new EmptyNode();
        if (iterator.Current.Type == TokenType.EqualsToken)
        {
            iterator.NextToken();

            initializer = Expression.Parse(parser);
        }

        iterator.Match(TokenType.Semicolon);

        return new VariableStatement(nameToken.Text, type, initializer, isMutable);
    }
}