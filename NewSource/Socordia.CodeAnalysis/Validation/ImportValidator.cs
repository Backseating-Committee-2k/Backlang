using MrKWatkins.Ast;
using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Statements;

namespace Socordia.CodeAnalysis.Validation;

public class ImportValidator : Validator<AstNode, RootBlock>
{
    protected override IEnumerable<Message> ValidateNode(RootBlock node)
    {
        for (var i = 0; i < node.Root.Children.Count; i++)
        {
            if (i > 0 && node.Root.Children[i - 1] is not ModuleDeclaration &&
                node.Root.Children[i - 1] is not ImportStatement)
            {
               // yield return new Message(MessageLevel.Warning, "Imports should be before module definition");
            }
        }
        
        foreach (var child in node.Root.Children)
        {
        }

        return [];
    }
}