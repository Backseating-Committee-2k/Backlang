using Backlang.Codeanalysis.Parsing;
using Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;
using Loyc.Syntax;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints.Declarations;

public sealed class FunctionDeclaration : IParsePoint
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var signature = Signature.Parse(parser);

        return signature.PlusArg(Statement.ParseBlock(parser)).WithRange(keywordToken, iterator.Prev);
    }
}