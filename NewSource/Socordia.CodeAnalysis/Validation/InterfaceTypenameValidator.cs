using MrKWatkins.Ast;
using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace Socordia.CodeAnalysis.Validation;

public class InterfaceTypenameValidator : Validator<AstNode, InterfaceDeclaration>
{
    protected override IEnumerable<Message> ValidateNode(InterfaceDeclaration node)
    {
        if (char.IsLower(node.Name[1]))
        {
            yield return new Message(MessageLevel.Error, "Type should be named uppercase");
        }

        if (node.Name[0] != 'I')
        {
            yield return new Message(MessageLevel.Error, "Interface has to start with I");
        }
    }
}