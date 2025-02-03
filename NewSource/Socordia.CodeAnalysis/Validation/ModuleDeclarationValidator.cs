using MrKWatkins.Ast;
using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace Socordia.CodeAnalysis.Validation;

public class ModuleDeclarationValidator : Validator<AstNode, ModuleDeclaration>
{
    protected override IEnumerable<Message> ValidateNode(ModuleDeclaration node)
    {
        var moduleDeclCount = node.Root.Children.OfType<ModuleDeclaration>().Count();

        if (moduleDeclCount > 1)
        {
            yield return new Message(MessageLevel.Error, "A module has more than one module declaration.");
        }
    }
}