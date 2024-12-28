using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class RulesForDeclarationParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;

        iterator.Match(TokenType.For);
        var target = TypeNameParser.Parse(parser);

        iterator.Match(TokenType.OpenCurly);

        List<AstNode> rules = [];

        iterator.Match(TokenType.CloseCurly); //remove if children parsing works

        return new RulesDeclaration(target, rules);
    }
}