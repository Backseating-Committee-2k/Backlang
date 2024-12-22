using Loyc.Syntax;
using Socordia.CodeAnalysis.AST.TypeNames;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints;

public sealed class TypeLiteralParser
{
    public static TypeName Parse(TokenIterator iterator, Parser parser)
    {
        TypeName typeNode;
        var typeToken = iterator.Current;

        if (iterator.IsMatch(TokenType.Identifier))
        {
            var typename = iterator.Match(TokenType.Identifier).Text;
            var args = new List<TypeName>();

            typeNode = new SimpleTypeName(typename);

            if (iterator.IsMatch(TokenType.Dot))
            {
                while (iterator.IsMatch(TokenType.Dot))
                {
                    iterator.NextToken();

                    var id = iterator.Match(TokenType.Identifier);
                    typeNode.Children.Add(new SimpleTypeName(id.Text));
                }
                typeNode = new QualifiedTypeName((SimpleTypeName)typeNode);
            }

            if (iterator.IsMatch(TokenType.Star))
            {
                typeNode = new PointerTypeName(typeNode, PointerKind.Transient);
            }

            if (iterator.IsMatch(TokenType.Ampersand))
            {
                typeNode = new PointerTypeName(typeNode, PointerKind.Reference);
            }

            /*
            else if (iterator.IsMatch(TokenType.Questionmark))
            {
                iterator.NextToken();

                typeNode = SyntaxTree.NullableType(typeNode).WithRange(typeToken, iterator.Prev);
            }
            else if (iterator.IsMatch(TokenType.OpenSquare))
            {
                typeNode = ParseArrayType(iterator, typeNode, typeToken);
            }*/
            else if (iterator.IsMatch(TokenType.LessThan))
            {
                typeNode = ParseGenericType(iterator, parser, typename, args);
            }
        }
        else if (iterator.IsMatch(TokenType.None))
        {
            typeNode = new SimpleTypeName("none");
            iterator.NextToken();
        }
        /*
        else if (iterator.IsMatch(TokenType.OpenParen))
        {
            typeNode = ParseFunctionOrTupleType(iterator, parser, typeToken);
        }*/
        else
        {
            parser.AddError("Expected Identifier, TupleType or Function-Signature as TypeLiteral, but got " +
                TokenIterator.GetTokenRepresentation(iterator.Current.Type));

            typeNode = null;
            iterator.NextToken();
        }

        return typeNode;
    }

    public static bool TryParse(Parser parser, out TypeName node)
    {
        var cursor = parser.Iterator.Position;
        node = Parse(parser.Iterator, parser);

        if (node == null)
        {
            parser.Iterator.Position = cursor;
            return false;
        }

        return true;
    }
/*
    private static LNode ParseFunctionOrTupleType(TokenIterator iterator, Parser parser, Token typeToken)
    {
        LNode typeNode;
        iterator.Match(TokenType.OpenParen);

        var parameters = new List<AstNode>();
        while (parser.Iterator.Current.Type != TokenType.CloseParen)
        {
            parameters.Add(Parse(iterator, parser));

            if (parser.Iterator.Current.Type != TokenType.CloseParen)
            {
                parser.Iterator.Match(TokenType.Comma);
            }
        }

        parser.Iterator.Match(TokenType.CloseParen);

        if (iterator.Current.Type == TokenType.Arrow)
        {
            iterator.NextToken();

            var returnType = Parse(iterator, parser);

            typeNode = SyntaxTree.Factory.Call(CodeSymbols.Fn,
                    LNode.List(returnType, LNode.Missing, LNode.Call(CodeSymbols.AltList, parameters)));
        }
        else
        {
            typeNode = SyntaxTree.Factory.Tuple(parameters);
        }

        return typeNode;
    }
*/
    private static TypeName ParseGenericType(TokenIterator iterator, Parser parser, string typename,
        List<TypeName> args)
    {
        TypeName typeNode;
        iterator.NextToken();

        while (!iterator.IsMatch(TokenType.GreaterThan))
        {
            if (iterator.IsMatch(TokenType.Identifier))
            {
                args.Add(Parse(iterator, parser));
            }

            if (!iterator.IsMatch(TokenType.GreaterThan))
            {
                iterator.Match(TokenType.Comma);
            }
        }

        iterator.Match(TokenType.GreaterThan);

        typeNode = new GenericTypeName(typename, args);
        return typeNode;
    }

    private static LNode ParseArrayType(TokenIterator iterator, LNode typeNode, Token typeToken)
    {
        iterator.NextToken();

        var dimensions = 1;

        while (iterator.IsMatch(TokenType.Comma))
        {
            dimensions++;

            iterator.NextToken();
        }

        iterator.Match(TokenType.CloseSquare);

        typeNode = SyntaxTree.Array(typeNode, dimensions).WithRange(typeToken, iterator.Prev);
        return typeNode;
    }
}