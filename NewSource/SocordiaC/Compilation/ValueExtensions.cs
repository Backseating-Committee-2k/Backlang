namespace Socordia.Compilation;

public static class ValueExtensions
{
    public static void EnsureType<T>(this Value value, AstNode node)
    {
        //ToDo: fix this
        if (value.Type != typeof(T))
        {
            node.AddError($"Expected value of type {type}, but got {value.Type}");
        }
    }
}