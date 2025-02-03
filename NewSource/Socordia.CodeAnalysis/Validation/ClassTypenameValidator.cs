using MrKWatkins.Ast;
using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace Socordia.CodeAnalysis.Validation;

public class ClassTypenameValidator : Validator<AstNode, ClassDeclaration>
{
    protected override IEnumerable<Message> ValidateNode(ClassDeclaration node)
    {
        if (char.IsLower(node.Name[0]))
        {
            yield return new Message(MessageLevel.Error, "Type should be named uppercase");
        }
    }
}