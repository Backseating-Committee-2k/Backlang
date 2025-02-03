using DistIL.IR;
using Socordia.CodeAnalysis.AST;

namespace Socordia.Compilation;

public static class ValueExtensions
{
    public static void EnsureType<T>(this Value value, AstNode node)
    {
        if (value.ResultType.ToString() != typeof(T).FullName)
        {
            node.AddError($"Expected value of type {typeof(T)}, but got {value.ResultType}");
        }
    }
}