using DistIL.AsmIO;
using DistIL.CodeGen.Cil;
using DistIL.IR;
using DistIL.IR.Utils;
using Flo;
using SocordiaC.Compilation;
using SocordiaC.Compilation.Body;
using SocordiaC.Core.Scoping;
using SocordiaC.Core.Scoping.Items;

namespace SocordiaC.Stages;

public class CompileFunctionsStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var (node, def) in Mappings.Functions)
        {
            var scope = new Scope(null!);
            foreach (var arg in def.Body!.Args)
            {
                scope.Add(new ParameterScopeItem
                {
                    Name = arg.Name,
                    Arg = arg
                });
            }

            var builder = new IRBuilder(def.Body!.CreateBlock());
            var bodyCompilation = new BodyCompilation(context, def, builder, scope);

            if (!node.Children[1].HasChildren)
            {
                builder.Emit(new ReturnInst());
            }
            else
            {
                BodyCompilation.Listener.Listen(bodyCompilation, node);

                if (node.IsExpressionBody && def.ReturnType == PrimType.Void)
                {
                    var ret = (ReturnInst)bodyCompilation.Method.Body.EntryBlock.First;
                    ret.ReplaceWith(ret.Value);
                }

                if (def.ReturnType == PrimType.Void)
                {
                    builder.Emit(new ReturnInst());
                }
            }

            def.ILBody = ILGenerator.GenerateCode(def.Body);
        }

        Mappings.Functions.Clear();

        return await next.Invoke(context);
    }
}