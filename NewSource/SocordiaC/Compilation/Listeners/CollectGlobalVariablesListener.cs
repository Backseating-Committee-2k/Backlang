using DistIL.AsmIO;
using DistIL.IR;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Literals;
using Socordia.CodeAnalysis.AST.Statements;
using Socordia.CodeAnalysis.AST.TypeNames;
using System.Reflection;

namespace SocordiaC.Compilation.Listeners;

public class CollectGlobalVariablesListener : Listener<Driver, AstNode, VariableStatement>
{
    protected override void ListenToNode(Driver context, VariableStatement node)
    {
        var type = context.GetGlobalsType(context.GetNamespaceOf(node));
        var varType = Utils.GetTypeFromNode(node.Type, type);
        var field = type.CreateField(node.Name, varType, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly | FieldAttributes.HasDefault);

        if (node.Initializer is LiteralNode literal)
        {
            var valueConst = Utils.CreateLiteral(literal.Value);

            SetValue(valueConst, field);
        }
        //Todo: add field value when its valetype then defaultvalue, otherwise use static ctor
    }

    private static void SetValue(Value valueConst, FieldDef field)
    {
        if (valueConst is ConstInt ci)
        {
            field.DefaultValue = ci.Value;
        }
        else if (valueConst is ConstFloat cf)
        {
            field.DefaultValue = cf.Value;
        }
        else if (valueConst is ConstString cs)
        {
            field.DefaultValue = cs.Value;
        }
    }

    protected override bool ShouldListenToChildren(Driver context, VariableStatement node)
    {
        return false;
    }
}
