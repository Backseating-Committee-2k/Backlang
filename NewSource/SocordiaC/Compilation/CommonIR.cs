using System.Reflection;
using DistIL.AsmIO;
using DistIL.CodeGen.Cil;
using DistIL.IR;
using DistIL.IR.Utils;
using MethodBody = DistIL.IR.MethodBody;

namespace SocordiaC.Compilation;

public class CommonIR
{
    private static readonly int[] Primes = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41];

    public static void GenerateGetHashCode(Driver context, TypeDef type)
    {
        var getHashCodeMethod = type.CreateMethod("GetHashCode", new TypeSig(PrimType.Int32), [],
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot);

        getHashCodeMethod.Body = new MethodBody(getHashCodeMethod);
        var builder = new IRBuilder(getHashCodeMethod.Body.CreateBlock());

        var hash = builder.Block.Method.CreateVar(PrimType.Int32, "hash");

        var startPrime = SelectPrime();
        var constant = ConstInt.CreateI(startPrime);

        builder.CreateStore(hash, constant);

        foreach (var field in type.Fields)
        {
            if (field.IsStatic) continue;

            var fieldValue = builder.CreateFieldLoad(field);
            var method = field.Type.Methods.FirstOrDefault(m => m.Name.ToString() == "GetHashCode");

            if (method != null)
            {
                var fieldHash = builder.CreateCall(method, fieldValue);
                builder.CreateStore(hash, builder.CreateBin(BinaryOp.Add, hash, fieldHash));
            }
            else if (field.Type is PrimType)
            {
                builder.CreateStore(hash, builder.CreateBin(BinaryOp.Add, hash, fieldValue));
            }
            else
            {
                var objectGetHashCode = context.Compilation.Resolver.SysTypes.Object.FindMethod("GetHashCode");
                var fieldHash = builder.CreateCall(objectGetHashCode, fieldValue);
                builder.CreateStore(hash, builder.CreateBin(BinaryOp.Add, hash, fieldHash));
            }

            var factor = ConstInt.CreateI(SelectPrime());
            builder.CreateStore(hash, builder.CreateBin(BinaryOp.Mul, hash, factor));
        }

        builder.Emit(new ReturnInst(hash));

        getHashCodeMethod.ILBody = ILGenerator.GenerateCode(builder.Method);
    }

    private static int SelectPrime()
    {
        return Primes[Random.Shared.Next(0, Primes.Length)];
    }

    public static MethodDef GenerateCtor(TypeDef type)
    {
        var parameters = new List<ParamDef> { new(new TypeSig(type), "this") };

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

        return ctor;
    }

    public static void GenerateEmptyCtor(TypeDef type)
    {
        var parameters = new List<ParamDef> { new(new TypeSig(type), "this") };

        var ctor = type.CreateMethod(".ctor", new TypeSig(PrimType.Void), [.. parameters],
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        ctor.Body = new MethodBody(ctor);
        var irBuilder = new IRBuilder(ctor.Body.CreateBlock());

        irBuilder.Emit(new ReturnInst());

        ctor.ILBody = ILGenerator.GenerateCode(ctor.Body);
    }
}