using Flo;
using Loyc.Syntax;
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
        Parallel.ForEachAsync(context.Settings.Sources, (filename, ct) => {
            if (File.Exists(filename))
            {
                var tree = CompilationUnit.FromFile(filename);

                ApplyTree(context, tree);
            }
            else
            {
                context.Messages.Add(Message.Error($"File '{filename}' does not exists", SourceRange.Synthetic));
            }

            return ValueTask.CompletedTask;
        }).Wait();
    }

    private static void ApplyTree(Driver context, CompilationUnit tree)
    {
        context.Trees.Add(tree);

        context.Messages.AddRange(tree.Messages);
    }
}