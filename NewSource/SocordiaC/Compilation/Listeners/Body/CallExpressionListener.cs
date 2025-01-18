using DistIL.AsmIO;
using DistIL.IR;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation.Listeners.Body;

public class CallExpressionListener(bool shouldEmit) : Listener<BodyCompilation, AstNode, CallExpression>
{
    public Instruction CallInstruction;

    protected override void ListenToNode(BodyCompilation context, CallExpression node)
    {
        var args = node.Arguments.Select(_ => Utils.CreateValue(_, context));

        if (CreateStaticExternalCall(context, node, args)) return;
        if (CreateStaticContainingTypeCalls(context, node, args)) return;
        if (CreatePrintCalls(context, node, args)) return;

        node.AddError("Function not found");
    }

    protected override void AfterListenToNode(BodyCompilation context, CallExpression node)
    {
        if (shouldEmit && CallInstruction.Block == null) context.Builder.Emit(CallInstruction);
    }

    private bool CreateStaticContainingTypeCalls(BodyCompilation context, CallExpression node, IEnumerable<Value> args)
    {
        var candidates = GetStaticMethodCandidates(node, args, context.Builder.Method.Definition.DeclaringType);
        if (candidates.Length == 0) return false;

        var method = candidates[0];
        CallInstruction = new CallInst(method, [.. args]);
        return true;
    }

    private bool CreatePrintCalls(BodyCompilation context, CallExpression node, IEnumerable<Value> args)
    {
        if (node.Callee.Name == "print")
        {
            var method = context.Driver.Compilation.Module.Resolver.FindMethod("System.Console::Write", [.. args]);
            CallInstruction = new CallInst(method, [.. args]);
            return true;
        }

        if (node.Callee.Name == "println")
        {
            var method = context.Driver.Compilation.Module.Resolver.FindMethod("System.Console::WriteLine", [.. args]);
            CallInstruction = new CallInst(method, [.. args]);
            return true;
        }

        return false;
    }

    private bool CreateStaticExternalCall(BodyCompilation context, CallExpression node, IEnumerable<Value> args)
    {
        if (node.Parent is BinaryOperatorExpression { Operator: "'::", Left: BinaryOperatorExpression typeNode })
        {
            var typename = new QualifiedTypeName(typeNode);
            var type = context.Driver.Compilation.Module.Resolver.FindType(typename.FullName);

            if (type == null)
            {
                typename.AddError("Type not found");
                return true;
            }

            var candidates = GetStaticMethodCandidates(node, args, type);

            if (candidates.Length == 0) node.AddError("No matching function found");

            var method = candidates[0];
            CallInstruction = new CallInst(method, [.. args]);
            return true;
        }

        return false;
    }

    private static MethodDesc[] GetStaticMethodCandidates(CallExpression node, IEnumerable<Value> args, TypeDesc type)
    {
        return type.Methods
            .Where(m => m.Name == node.Callee.Name && m.IsStatic)
            .Where(m => m.ParamSig.Count == node.Arguments.Count())
            .Where(m => m.ParamSig.Zip(
                args,
                (p, a) => p.Type.IsAssignableTo(a.ResultType)).All(x => x)).ToArray();
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, AstNode node)
    {
        return false;
    }
}