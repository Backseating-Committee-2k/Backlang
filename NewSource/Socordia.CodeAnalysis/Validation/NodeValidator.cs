using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Validation;

public static class NodeValidator
{
    public static readonly Pipeline<AstNode> Pipeline =
        Pipeline<AstNode>
            .Build(
                builder => {
                    builder.AddStage<OperatorDefinitionValidator>();
                    builder.AddStage<ClassTypenameValidator>();
                    builder.AddStage<InterfaceTypenameValidator>();
                    builder.AddStage<ImportValidator>();
                    builder.AddStage<ModuleDeclarationValidator>();
                });
}