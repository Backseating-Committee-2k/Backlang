using MrKWatkins.Ast;
using MrKWatkins.Ast.Processing;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace Socordia.CodeAnalysis.Validation;

public class OperatorDefinitionValidator : Validator<AstNode, FunctionDefinition>
{
    protected override IEnumerable<Message> ValidateNode(FunctionDefinition node)
    {
        if (node.Parent is not RootBlock)
        {
            return [];
        }

        if (node.Modifiers.Contains(Modifier.Operator))
        {
            return [new Message(MessageLevel.Error, "Operator cannot be defined as free function")];
        }

        return [];
    }
}