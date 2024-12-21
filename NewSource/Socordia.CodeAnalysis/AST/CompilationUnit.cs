using Loyc.Syntax;
using Socordia.CodeAnalysis.Parsing;

namespace Socordia.CodeAnalysis.AST;

public sealed class CompilationUnit
{
    public List<Declaration> Declarations { get; set; } = [];
    public SourceFile<StreamCharSource> Document { get; internal set; }
    public List<Message> Messages { get; set; } = [];

    public static CompilationUnit FromFile(string filename)
    {
        var document = new SourceDocument(filename);

        return Parser.Parse(document);
    }

    public static CompilationUnit FromText(string text)
    {
        var document = new SourceDocument("inline.back", text);

        return Parser.Parse(document);
    }
}