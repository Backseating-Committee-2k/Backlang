using DistIL.AsmIO;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public static class Mappings
{
    public static Dictionary<FunctionDefinition, MethodDef> Functions = [];
    public static Dictionary<ClassDeclaration, TypeDef> Types = [];
}