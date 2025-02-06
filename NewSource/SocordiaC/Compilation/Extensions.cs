using DistIL.AsmIO;
using DistIL.IR;
using Socordia.CodeAnalysis.AST;
using System.Runtime.CompilerServices;

namespace Socordia.Compilation;

public static class Extensions
{
    public static void EnsureType<T>(this Value value, AstNode node)
    {
        if (value.ResultType.ToString() != typeof(T).FullName)
        {
            node.AddError($"Expected value of type {typeof(T)}, but got {value.ResultType}");
        }
    }

    public static void AddCompilerGeneratedAttribute(this ModuleEntity entity, DistIL.Compilation compilation)
    {
        entity.GetCustomAttribs(false)
            .Add(new CustomAttrib(
                    compilation.Resolver.Import(typeof(CompilerGeneratedAttribute))
                        .FindMethod(".ctor")
                )
            );
    }
}