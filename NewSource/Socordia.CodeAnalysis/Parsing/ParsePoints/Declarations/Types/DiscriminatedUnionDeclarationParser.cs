using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Declarations.DU;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public class DiscriminatedUnionDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var nameToken = iterator.Match(TokenType.Identifier);
        iterator.Match(TokenType.EqualsToken);

        var types = new List<DiscriminatedType>();

        do
        {
            //ToDo: Refactor To Helper Method: RepeatUntil
            var from = iterator.Current;
            iterator.Match(TokenType.Pipe);
            types.Add(ParseType(iterator, parser));
        } while (iterator.IsMatch(TokenType.Pipe));

        iterator.Match(TokenType.Semicolon);

        return new DiscriminatedUnionDeclaration(nameToken.Text, types);
    }

    public static DiscriminatedType ParseType(TokenIterator iterator, Parser parser)
    {
        var nameToken = iterator.Match(TokenType.Identifier);

        iterator.Match(TokenType.OpenParen);

        var parameters = ParseParameterDeclarations(iterator, parser);

        iterator.Match(TokenType.CloseParen);

        return new DiscriminatedType(nameToken.Text, parameters);
    }

    public static List<ParameterDeclaration> ParseParameterDeclarations(TokenIterator iterator, Parser parser)
    {
        var parameters = new List<ParameterDeclaration>();
        while (iterator.Current.Type != TokenType.CloseParen)
        {
            while (iterator.Current.Type != TokenType.Comma && iterator.Current.Type != TokenType.CloseParen)
            {
                var parameter = (ParameterDeclaration)ParameterDeclarationParser.Parse(iterator, parser);

                if (iterator.Current.Type == TokenType.Comma)
                {
                    iterator.NextToken();
                }

                parameters.Add(parameter);
            }
        }

        return parameters;
    }
}