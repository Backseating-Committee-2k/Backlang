using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;
using Loyc.Syntax;

namespace BacklangC.Core.Semantic.Checks;

internal class ModuleDefinitionCheck : ISemanticCheck
{
    public void Check(CompilationUnit tree, Driver context)
    {
        if (tree.Declarations.Count(_ => _.Calls(CodeSymbols.Namespace)) > 1)
        {
            var moduleDefinition = tree.Declarations.First(_ => _.Calls(CodeSymbols.Namespace));

            context.Messages.Add(Message.Warning("A module definition is already defined", moduleDefinition.Range));
        }
    }
}