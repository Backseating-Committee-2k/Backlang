using DistIL.AsmIO;
using DistIL.IR;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;
using Socordia.CodeAnalysis.AST.TypeNames;
using SocordiaC.Compilation.Scoping.Items;

namespace SocordiaC.Compilation.Listeners.Body;

public class VariableDeclarationListener : Listener<BodyCompilation, AstNode, VariableStatement>
{
    protected override void ListenToNode(BodyCompilation context, VariableStatement node)
    {
        var value = Utils.CreateValue(node.Initializer, context);
        if (value is Instruction { Block: null } instr)
        {
            context.Builder.Emit(instr);
        }

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

            if (value is LocalSlot s)
            {
                //   type = s.Type;
            }
        }

        if (value is ConstNull && !node.IsMutable)
        {
            node.AddError("Cannot declare a non mutable variable with a null value");
        }

        var slot = context.Method.Body!.CreateVar(type, node.Name);

        context.Scope.Add(new VariableScopeItem { Slot = slot, Name = node.Name, IsMutable = node.IsMutable });

        context.Builder.CreateStore(slot, value);
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, VariableStatement node)
    {
        return false;
    }
}