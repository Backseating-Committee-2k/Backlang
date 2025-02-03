using Backlang.Codeanalysis.Core;
using Backlang.Contracts.Scoping.Items;
using Backlang.Driver.Core.Implementors;
using Backlang.Driver.Core.Implementors.Expressions;
using Backlang.Driver.Core.Implementors.Statements;
using Furesoft.Core.CodeDom.Compiler.Core.Collections;
using Furesoft.Core.CodeDom.Compiler.Flow;
using Furesoft.Core.CodeDom.Compiler.Instructions;
using Furesoft.Core.CodeDom.Compiler.TypeSystem;

namespace Backlang.Driver.Compiling.Stages.CompilationStages;

public partial class ImplementationStage
{
    

    public static BasicBlockBuilder AppendBlock(LNode blkNode, BasicBlockBuilder block, CompilerContext context,
        IMethod method, QualifiedName? modulename, Scope scope, BranchLabels branchLabels)
    {
        block.Flow = new NothingFlow();

        foreach (var node in blkNode.Args)
        {
            if (!node.IsCall)
            {
                continue;
            }

            if (node.Calls(CodeSymbols.Braces))
            {
                if (node.ArgCount == 0)
                {
                    continue;
                }

                block = AppendBlock(node, block.Graph.AddBasicBlock(), context, method, modulename,
                    scope.CreateChildScope(), branchLabels);
                continue;
            }

            if (_implementations.ContainsKey(node.Name))
            {
                block = _implementations[node.Name]
                    .Implement(node, block, context, method, modulename, scope, branchLabels);
            }
            else
            {
                EmitFunctionCall(method, node, block, context, scope, modulename);
            }
        }

        //automatic dtor call
        AppendAllDtors(block, context, modulename, scope);

        return block;
    }

    public static NamedInstructionBuilder AppendDtor(CompilerContext context, BasicBlockBuilder block, Scope scope,
        QualifiedName? modulename, string varname)
    {
        if (scope.TryGet<VariableScopeItem>(varname, out var scopeItem))
        {
            if (!scopeItem.Type.Methods.Any(_ => _.Name.ToString() == "Finalize"))
            {
                return null;
            }

            block.AppendInstruction(Instruction.CreateLoadLocal(scopeItem.Parameter));

            return AppendCall(context, block, LNode.Missing, scopeItem.Type.Methods, scope, modulename, false,
                "Finalize");
        }

        return null;
    }

    public static void AppendAllDtors(BasicBlockBuilder block, CompilerContext context, QualifiedName? modulename,
        Scope scope)
    {
        foreach (var v in block.Parameters)
        {
            AppendDtor(context, block, scope, modulename, v.Tag.Name);
        }
    }

    private static void SetReturnType(DescribedBodyMethod method, LNode function, CompilerContext context, Scope scope,
        QualifiedName modulename)
    {
        var retType = function.Args[0];

        if (retType.Name != LNode.Missing.Name)
        {
            var rtype = TypeInheritanceStage.ResolveTypeWithModule(retType, context, modulename);

            method.ReturnParameter = new Parameter(rtype);
        }
        else
        {
            var deducedReturnType = TypeDeducer.DeduceFunctionReturnType(function, context, scope, modulename);

            method.ReturnParameter = deducedReturnType != null
                ? new Parameter(deducedReturnType)
                : new Parameter(Utils.ResolveType(context.Binder, typeof(void)));
        }
    }

    private static BasicBlockBuilder EmitFunctionCall(IMethod method, LNode node, BasicBlockBuilder block,
        CompilerContext context, Scope scope, QualifiedName? moduleName)
    {
        //ToDo: continue implementing static function call in same type
        var type = (DescribedType)method.ParentType;
        var calleeName = node.Target;
        var methods = type.Methods;

        if (!methods.Any(_ => _.Name.ToString() == calleeName.ToString()))
        {
            type = (DescribedType)context.Binder
                .ResolveTypes(new SimpleName(Names.FreeFunctions).Qualify(moduleName.Value)).FirstOrDefault();

            if (type == null)
            {
                context.AddError(node, new LocalizableString(ErrorID.CannotFindFunction, calleeName.ToString()));
            }
        }

        if (scope.TryGet<FunctionScopeItem>(calleeName.Name.Name, out var callee))
        {
            if (type.IsStatic && !callee.IsStatic)
            {
                context.AddError(node,
                    $"A non static function '{calleeName.Name.Name}' cannot be called in a static function.");
                return block;
            }

            AppendCall(context, block, node, type.Methods, scope, moduleName.Value);
        }
        else
        {
            var suggestion =
                LevensteinDistance.Suggest(calleeName.Name.Name, type.Methods.Select(_ => _.Name.ToString()));

            context.AddError(node,
                new LocalizableString(ErrorID.CannotBeFoundDidYouMean, calleeName.Name.Name, suggestion));
        }

        return block;
    }
}