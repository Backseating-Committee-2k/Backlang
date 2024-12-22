using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class FunctionDefinitionParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var signature = SignatureParser.Parse(parser);

        return new FunctionDefinition(signature, Statements.Statement.ParseBlock(parser));
    }
}
