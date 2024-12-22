using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class TypeFunctionDeclaration
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Match(TokenType.Function);
        var result = SignatureParser.Parse(parser);
        iterator.Match(TokenType.Semicolon);

        return result;
    }
}