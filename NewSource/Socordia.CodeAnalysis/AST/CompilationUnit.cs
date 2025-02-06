using MrKWatkins.Ast.Position;
using Socordia.CodeAnalysis.Parsing;

namespace Socordia.CodeAnalysis.AST;

public sealed class CompilationUnit
{
    public RootBlock Declarations { get; set; } = new([]);
    public TextFile Document { get; internal set; }
    public List<Message> Messages { get; set; } = [];

    public static CompilationUnit FromFile(string filename)
    {
        var document = new TextFile(filename, File.ReadAllText(filename));

        return Parser.Parse(document);
    }

    public static CompilationUnit FromText(string text)
    {
        return Parser.Parse(new TextFile("inline.sc", text));
    }
}