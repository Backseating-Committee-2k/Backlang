namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

/*
public sealed class FunctionDeclaration : IParsePoint
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var signature = Signature.Parse(parser);

        return signature.PlusArg(Statements.Statement.ParseBlock(parser)).WithRange(keywordToken, iterator.Prev);
    }
}
*/