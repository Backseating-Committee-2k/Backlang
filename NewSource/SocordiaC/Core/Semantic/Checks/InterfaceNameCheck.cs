using Loyc.Syntax;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.Parsing;

namespace SocordiaC.Core.Semantic.Checks;

internal class InterfaceNameCheck : ISemanticCheck
{
    public void Check(CompilationUnit tree, Driver context)
    {
        for (var i = 0; i < tree.Declarations.Count; i++)
        {
            var node = tree.Declarations[i];

            if (node.Calls(CodeSymbols.Interface))
            {
                if (node is var (_, typename, _) && typename.Name.Name.Length >= 2)
                {
                    if (typename.Name.Name[0] != 'I')
                    {
                        context.Messages.Add(Message.Warning($"Interface '{typename.Name.Name}' should be start with I",
                            node.Range));
                    }

                    if (char.IsLower(typename.Name.Name[1]))
                    {
                        context.Messages.Add(Message.Warning(
                            $"The second letter of interface '{typename.Name.Name}' should be uppercase", node.Range));
                    }
                }
            }
        }
    }
}