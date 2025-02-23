﻿using Backlang.Contracts.Scoping.Items;
using static Backlang.Driver.Compiling.Stages.CompilationStages.ImplementationStage;

namespace Backlang.Driver.Core.Implementors.Statements;

public class AssignmentImplementor : IStatementImplementor
{
    public BasicBlockBuilder Implement(LNode node, BasicBlockBuilder block, CompilerContext context, IMethod method,
        QualifiedName? modulename, Scope scope, BranchLabels branchLabels = null)
    {
        if (node is var (_, left, right))
        {
            var lt = TypeDeducer.Deduce(left, scope, context, modulename.Value);
            var rt = TypeDeducer.Deduce(right, scope, context, modulename.Value);
            TypeDeducer.ExpectType(right, scope, context, modulename.Value, lt);

            if (scope.TryGet<VariableScopeItem>(left.Name.Name, out var va))
            {
                if (!va.IsMutable)
                {
                    context.AddError(node, $"Cannot assing immutable variable '{va.Name}'");
                }

                var value = AppendExpression(block, right, rt, context,
                    scope, modulename.Value);
                var pointer = block.AppendInstruction(Instruction.CreateLoadLocal(va.Parameter));

                block.AppendInstruction(Instruction.CreateStore(lt, pointer, value));
            }
            else if (left is ("'.", var t, var c))
            {
                if (scope.TryGet<VariableScopeItem>(t.Name.Name, out var vsi))
                {
                    var field = vsi.Type.Fields.FirstOrDefault(_ => _.Name.ToString() == c.Name.Name);

                    if (field != null)
                    {
                        block.AppendInstruction(Instruction.CreateLoadLocalAdress(vsi.Parameter));
                        var value = AppendExpression(block, right, field.FieldType,
                            context, scope, modulename.Value);

                        var pointer = block.AppendInstruction(Instruction.CreateLoadField(field));

                        block.AppendInstruction(Instruction.CreateStore(field.FieldType, pointer, value));
                    }
                }
            }
            else
            {
                context.AddError(node, $"Variable '{left.Name.Name}' not found");
            }
        }

        return block;
    }
}