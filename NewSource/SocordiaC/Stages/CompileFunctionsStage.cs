using DistIL.AsmIO;
using DistIL.CodeGen.Cil;
using DistIL.IR;
using DistIL.IR.Utils;
using Flo;
using Socordia.CodeAnalysis.AST.Declarations;
using SocordiaC.Compilation;
using SocordiaC.Compilation.Listeners.Body;
using SocordiaC.Compilation.Scoping;
using SocordiaC.Compilation.Scoping.Items;

namespace SocordiaC.Stages;

public class CompileFunctionsStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var (node, def) in Mappings.Functions)
        {
            var scope = new Scope(null!);
            foreach (var arg in def.Body!.Args)
                scope.Add(new ParameterScopeItem
                {
                    Name = arg.Name,
                    Arg = arg
                });

            var builder = new IRBuilder(def.Body!.CreateBlock());
            EmitParameterNullChecks(builder, node, context);
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
                    var ret = (ReturnInst)bodyCompilation.Method.Body!.EntryBlock.First;
                    ret.ReplaceWith(ret.Value!);
                }

                if (def.ReturnType == PrimType.Void) builder.Emit(new ReturnInst());
            }

            def.ILBody = ILGenerator.GenerateCode(def.Body);
        }

        return await next.Invoke(context);
    }

    private void EmitParameterNullChecks(IRBuilder builder, FunctionDefinition node, Driver driver)
    {
        for (int i = 0; i < node.Signature.Parameters.Count(); i++)
        {
            var parameter = node.Signature.Parameters.Skip(i).First();

            if (!parameter.AssertNotNull) continue;

            var cmp = builder.CreateCmp(CompareOp.Ne, builder.Method.Args[i], ConstNull.Create());
            var ctor = driver.KnownTypes.ArgumentNullExceptionType!.FindMethod(".ctor", new MethodSig(PrimType.Void, [new TypeSig(PrimType.String)]));

            builder.ForkIf(cmp, (irBuilder, _) =>
            {
                var exception = irBuilder.CreateNewObj(ctor, ConstString.Create($"{parameter.Name} cannot be null"));
                irBuilder.Emit(new ThrowInst(exception));
            });
        }
    }
}