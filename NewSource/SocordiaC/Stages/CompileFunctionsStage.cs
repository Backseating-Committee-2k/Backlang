using DistIL.CodeGen.Cil;
using DistIL.IR;
using DistIL.IR.Utils;
using Flo;
using SocordiaC.Compilation;

namespace SocordiaC.Stages;

public class CompileFunctionsStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var (node, def) in Mappings.Functions)
        {
            var builder = new IRBuilder(def.Body!.CreateBlock());

            if (!node.Children[1].HasChildren)
            {
                builder.Emit(new ReturnInst());
            }

            def.ILBody = ILGenerator.GenerateCode(def.Body);
        }

        Mappings.Functions.Clear();

        return await next.Invoke(context);
    }
}