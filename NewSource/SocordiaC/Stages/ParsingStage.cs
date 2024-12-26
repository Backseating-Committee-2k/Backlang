using Flo;
using Loyc.Syntax;
using MrKWatkins.Ast.Position;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.Parsing;

namespace SocordiaC.Stages;

public sealed class ParsingStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context,
        Func<Driver, Task<Driver>> next)
    {
        ParseSourceFiles(context);

        return await next.Invoke(context);
    }

    private static void ParseSourceFiles(Driver context)
    {
        foreach (var filename in context.Settings.Sources)
        {
            if (File.Exists(filename))
            {
                var tree = CompilationUnit.FromFile(filename);

                ApplyTree(context, tree);
            }
            else
            {
               // context.Messages.Add(Message.Error($"File '{filename}' does not exists", (TextFilePosition)TextFilePosition.None));
            }
        }
    }

    private static void ApplyTree(Driver context, CompilationUnit tree)
    {
        context.Trees.Add(tree);

        context.Messages.AddRange(tree.Messages);
    }
}