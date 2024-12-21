using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class ModuleDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        //module <identifier>
        //module <identifier>.<identifier>
        var keywordToken = iterator.Prev;
        var tree = SyntaxTree.Module(Expression.Parse(parser));

        iterator.Match(TokenType.Semicolon);

        return tree.WithRange(keywordToken, iterator.Prev);
    }
}