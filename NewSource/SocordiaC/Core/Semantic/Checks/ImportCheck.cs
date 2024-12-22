using Loyc.Syntax;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.Parsing;

namespace SocordiaC.Core.Semantic.Checks;

/*
internal class ImportCheck : ISemanticCheck
{
    public void Check(CompilationUnit tree, Driver context)
    {
        for (var i = 0; i < tree.Declarations.Count; i++)
        {
            var node = tree.Declarations[i];

            if (i > 0 && !tree.Declarations[i - 1].Calls(CodeSymbols.Namespace) &&
                !tree.Declarations[i - 1].Calls(CodeSymbols.Import) && node.Calls(CodeSymbols.Import))
            {
                context.Messages.Add(Message.Warning("Imports should be before module definition", node.Range));
            }
        }
    }
}
*/