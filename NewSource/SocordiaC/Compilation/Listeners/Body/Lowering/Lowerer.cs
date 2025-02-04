using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;

namespace SocordiaC.Compilation.Listeners.Body.Lowering;

public static class Lowerer
{
    public static readonly Pipeline<AstNode> Pipeline =
        Pipeline<AstNode>
            .Build(
                builder =>
                {
                    builder.AddStage<AssignmentsLowerer>();
                    builder.AddStage<SwapLowerer>();
                });
}