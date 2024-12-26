using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Statements;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class FunctionDefinitionParser : IParsePoint
{
    public static AstNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var signature = SignatureParser.Parse(parser);

        Block body = null;
        if (iterator.IsMatch(TokenType.OpenCurly))
        {
            body = Statements.Statement.ParseBlock(parser);
        }
        else if (iterator.ConsumeIfMatch(TokenType.Arrow))
        {
            body = new Block([new ReturnStatement(Expression.Parse(parser))]);
            iterator.Match(TokenType.Semicolon);
        }
        else
        {
            signature.AddError("Function body is missing");
        }

        return new FunctionDefinition(signature, body);
    }
}
