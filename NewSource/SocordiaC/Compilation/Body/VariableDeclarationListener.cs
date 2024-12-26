using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation.Body;

public class VariableDeclarationListener : Listener<BodyCompilation, AstNode, VariableStatement>
{
    protected override void ListenToNode(BodyCompilation context, VariableStatement node)
    {
        var value = Utils.CreateValue(node.Initializer);

        TypeDesc type;
        if (node.Type is not NoTypeName)
        {
            type = Utils.GetTypeFromNode(node.Type, context.Driver.Compilation.Module)!;
            if (type != value.ResultType)
            {
                node.Type.AddError("Type mismatch");
            }
        }
        else
        {
            type = value.ResultType;
        }

        var slot = context.Method.Body!.CreateVar(type!, node.Name);

        context.Builder.CreateStore(slot, value);
    }
}