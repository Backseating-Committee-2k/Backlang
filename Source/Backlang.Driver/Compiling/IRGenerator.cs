using Furesoft.Core.CodeDom.Compiler.Core.Collections;
using Furesoft.Core.CodeDom.Compiler.Core.Constants;
using Furesoft.Core.CodeDom.Compiler.Flow;
using Furesoft.Core.CodeDom.Compiler.Instructions;
using Furesoft.Core.CodeDom.Compiler.TypeSystem;

namespace Backlang.Driver.Compiling;

public static class IRGenerator
{
    public static void GenerateToString(CompilerContext context, DescribedType type)
    {
        var toStringMethod = new DescribedBodyMethod(type, new SimpleName("ToString"), false,
            Utils.ResolveType(context.Binder, typeof(string)));
        toStringMethod.IsPublic = true;
        toStringMethod.IsOverride = true;

        var graph = Utils.CreateGraphBuilder();

        var block = graph.EntryPoint;

        var sbType = Utils.ResolveType(context.Binder, typeof(StringBuilder));
        var varname = Utils.GenerateIdentifier();
        var p = block.AppendParameter(new BlockParameter(sbType, varname));

        var ctor = sbType.Methods.First(_ => _.IsConstructor && _.Parameters.Count == 0);

        var appendLineMethod = context.Binder.FindFunction("System.Text.StringBuilder::AppendLine(System.String)");

        block.AppendInstruction(Instruction.CreateNewObject(ctor, new List<ValueTag>()));
        block.AppendInstruction(Instruction.CreateAlloca(sbType));

        var loadSb = block.AppendInstruction(Instruction.CreateLoadLocal(new Parameter(p.Type, p.Tag.Name)));

        AppendLine(context, block, appendLineMethod, loadSb, $"{type.FullName}:");

        foreach (var field in type.Fields)
        {
            //AppendThis(block, p.Type);

            var loadSbf = block.AppendInstruction(Instruction.CreateLoadLocal(new Parameter(p.Type, p.Tag.Name)));

            AppendLine(context, block, appendLineMethod, loadSbf, field.Name + " = ");
            var value = AppendLoadField(block, field);

            var appendMethod =
                context.Binder.FindFunction($"System.Text.StringBuilder::Append({field.FieldType.FullName})");

            appendMethod ??= context.Binder.FindFunction("System.Text.StringBuilder::Append(System.Object)");

            block.AppendInstruction(Instruction.CreateCall(appendMethod, MethodLookup.Virtual,
                new List<ValueTag> { loadSbf, value }));
        }

        var tsM = context.Binder.FindFunction("System.Text.StringBuilder::ToString()");
        block.AppendInstruction(Instruction.CreateCall(tsM, MethodLookup.Virtual, new List<ValueTag> { loadSb }));

        block.Flow = new ReturnFlow();

        toStringMethod.Body = new MethodBody(new Parameter(), new Parameter(type), EmptyArray<Parameter>.Value,
            graph.ToImmutable());

        type.AddMethod(toStringMethod);
    }

    private static void AppendLine(CompilerContext context, BasicBlockBuilder block, IMethod appendLineMethod,
        NamedInstructionBuilder argLoad, string valueStr)
    {
        var va = block.AppendInstruction(
            Instruction.CreateConstant(new StringConstant(valueStr), context.Environment.String));
        var value = block.AppendInstruction(Instruction.CreateLoad(context.Environment.String, va));

        block.AppendInstruction(Instruction.CreateCall(appendLineMethod, MethodLookup.Virtual,
            new List<ValueTag> { argLoad, value }));
    }
}