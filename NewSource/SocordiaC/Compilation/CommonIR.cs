using System.Reflection;
using DistIL.AsmIO;
using DistIL.CodeGen.Cil;
using DistIL.IR;
using DistIL.IR.Utils;
using MethodBody = DistIL.IR.MethodBody;

namespace SocordiaC.Compilation;

public class CommonIR
{
    public static void GenerateCtor(TypeDef type)
    {
        var parameters = new List<ParamDef> { new ParamDef(new TypeSig(type), "this") };

        foreach (var field in type.Fields)
        {
            parameters.Add(new ParamDef(new TypeSig(field.Type), field.Name));
        }

        var ctor = type.CreateMethod(".ctor", new TypeSig(PrimType.Void), [.. parameters],
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        ctor.Body = new MethodBody(ctor);
        var irBuilder = new IRBuilder(ctor.Body.CreateBlock());

        foreach (var arg in ctor.Body.Args.Skip(1))
        {
            irBuilder.CreateFieldStore(type.FindField(arg.Name), ctor.Body.Args[0], arg);
        }

        irBuilder.Emit(new ReturnInst());

        ctor.ILBody = ILGenerator.GenerateCode(ctor.Body);
    }

    public static void GenerateEmptyCtor(TypeDef type)
    {
        var parameters = new List<ParamDef> { new ParamDef(new TypeSig(type), "this") };

        var ctor = type.CreateMethod(".ctor", new TypeSig(PrimType.Void), [.. parameters],
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        ctor.Body = new MethodBody(ctor);
        var irBuilder = new IRBuilder(ctor.Body.CreateBlock());

        irBuilder.Emit(new ReturnInst());

        ctor.ILBody = ILGenerator.GenerateCode(ctor.Body);
    }
}