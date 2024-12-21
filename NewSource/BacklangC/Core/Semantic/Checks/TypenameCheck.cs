using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;
using Loyc.Syntax;

namespace BacklangC.Core.Semantic.Checks;

internal class TypenameCheck : ISemanticCheck
{
    public void Check(CompilationUnit tree, Driver context)
    {
        for (var i = 0; i < tree.Declarations.Count; i++)
        {
            var node = tree.Declarations[i];

            if (node.Calls(CodeSymbols.Class) || node.Calls(CodeSymbols.Struct))
            {
                if (node is var (_, typename, _) && char.IsLower(typename.Name.Name[0]))
                {
                    context.Messages.Add(
                        Message.Warning($"Type '{typename.Name.Name}' should be Uppercase", node.Range));
                }
            }
        }
    }
}