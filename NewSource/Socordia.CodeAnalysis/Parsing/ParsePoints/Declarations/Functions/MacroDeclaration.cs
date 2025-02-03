namespace Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

/*
public sealed class MacroDeclaration : IParsePoint
{
    public static LNode Parse(TokenIterator iterator, Parser parser)
    {
        var keywordToken = iterator.Prev;
        var signature = Signature.Parse(parser);

        return signature.WithTarget(Symbols.Macro).PlusArg(LNode.Call(CodeSymbols.Braces, Statements.Statement.ParseBlock(parser))
            .SetStyle(NodeStyle.StatementBlock)).WithRange(keywordToken, iterator.Prev);
    }
}
*/