using DistIL.AsmIO;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public static class Mappings
{
    public static readonly Dictionary<FunctionDefinition, MethodDef> Functions = [];
    public static readonly Dictionary<ClassDeclaration, TypeDef> Types = [];
}