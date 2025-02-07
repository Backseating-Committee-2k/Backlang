namespace SocordiaC.Compilation.Listeners;

public class CollectGlobalVariablesListener : Listeners<Driver, AstNode, VariableStatement>
{
    protected override void ListenToNode(Driver context, VariableStatement node)
    {
        var type = context.GetFunctionType(context.GetNamespaceOf(node));
        var varType = Utils.GetTypeFromNode(node.Type, type);
        var field = type.CreateField(node.Name, varType, FieldAttributes.Public | FieldAttributes.Static);

        Mappings.Variables.Add(node, field);
    }

    protected override bool ShouldListenToChildren(Driver context, VariableStatement node)
    {
        return false;
    }
}
