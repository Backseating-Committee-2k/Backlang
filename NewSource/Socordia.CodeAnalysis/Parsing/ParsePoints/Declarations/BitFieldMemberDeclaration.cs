using Loyc.Syntax;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class BitFieldMemberDeclaration : IParsePoint
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        Token nameToken;
        if (iterator.Current.Type == TokenType.Identifier)
        {
            nameToken = iterator.Current;

            iterator.NextToken();
        }
        else
        {
            nameToken = iterator.NextToken();
        }

        iterator.Match(TokenType.EqualsToken);

        var value = Expression.Parse(parser);

        if (!value[0].HasValue)
        {
            parser.AddError(ErrorID.BitfieldNotLiteral, value.Range);
        }

        return SyntaxTree.Factory.Tuple(SyntaxTree.Factory.Id(nameToken.Text).WithRange(nameToken), value);
    }
}