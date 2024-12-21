using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Declarations;

public class UnitDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var nameToken = iterator.Match(TokenType.Identifier);

        iterator.Match(TokenType.Semicolon);

        return SyntaxTree.UnitDeclaration(nameToken).WithRange(keywordToken, nameToken);
    }
}