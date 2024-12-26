using DistIL.CodeGen.Cil;
using DistIL.IR;
using DistIL.IR.Utils;
using Flo;
using SocordiaC.Compilation;
using SocordiaC.Compilation.Body;

namespace SocordiaC.Stages;

public class CompileFunctionsStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var (node, def) in Mappings.Functions)
        {
            var builder = new IRBuilder(def.Body!.CreateBlock());
            var bodyCompilation = new BodyCompilation(context, def, builder);

            if (!node.Children[1].HasChildren)
            {
                builder.Emit(new ReturnInst());
            }
            else
            {
                BodyCompilation.Listener.Listen(bodyCompilation, node);
                builder.Emit(new ReturnInst());
            }

            def.ILBody = ILGenerator.GenerateCode(def.Body);
        }

        Mappings.Functions.Clear();

        return await next.Invoke(context);
    }
}