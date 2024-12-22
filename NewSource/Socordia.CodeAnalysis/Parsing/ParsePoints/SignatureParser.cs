using Loyc.Syntax;
using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints;

public sealed class SignatureParser
{
    public static Signature Parse(Parser parser)
    {
        var iterator = parser.Iterator;

        var nameToken = iterator.NextToken();
        var name = new Identifier(nameToken.Text);

        AstNode? returnType = null;
        iterator.Match(TokenType.OpenParen);

        var parameters = Declarations.ParameterDeclarationParser.ParseList(parser);

        var generics = new List<AstNode>();
        while (iterator.IsMatch(TokenType.Where))
        {
            iterator.NextToken();
            var genericName = LNode.Id(iterator.Match(TokenType.Identifier).Text);
            iterator.Match(TokenType.Colon);
            var bases = new List<AstNode>();
            do
            {
                if (iterator.IsMatch(TokenType.Comma))
                {
                    iterator.NextToken();
                }

                bases.Add(TypeLiteralParser.Parse(iterator, parser));
            } while (iterator.IsMatch(TokenType.Comma));

            //generics.Add(LNode.Call(Symbols.Where, LNode.List(genericName, LNode.Call(CodeSymbols.Base, bases))));
        }

        if (iterator.IsMatch(TokenType.Arrow))
        {
            iterator.NextToken();

            returnType = TypeLiteralParser.Parse(iterator, parser);
        }

        return SyntaxTree.Signature(name, returnType, parameters, generics);
    }
}