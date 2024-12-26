using DistIL.AsmIO;
using DistIL.IR;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation.Body;

public class CallExpressionListener : Listener<BodyCompilation, AstNode, CallExpression>
{
    protected override void ListenToNode(BodyCompilation context, CallExpression node)
    {
        var args = node.Arguments.Select(Utils.CreateValue);

        if (CreateStaticExternalCall(context, node, args)) return;
        if (CreateStaticContainingTypeCalls(context, node, args)) return;
        if (CreatePrintCalls(context, node, args)) return;

        node.AddError("Function not found");
    }

    private static bool CreateStaticContainingTypeCalls(BodyCompilation context, CallExpression node, IEnumerable<Value> args)
    {
        var candidates = GetStaticMethodCandidates(node, args, context.Builder.Method.Definition.DeclaringType);
        if (candidates.Length == 0)
        {
            return false;
        }

        var method = candidates[0];
        context.Builder.CreateCall(method, [.. args]);
        return true;
    }

    private static bool CreatePrintCalls(BodyCompilation context, CallExpression node, IEnumerable<Value> args)
    {
        if (node.Callee.Name == "print")
        {
            var method = context.Driver.Compilation.Module.Resolver.FindMethod("System.Console::Write", [.. args]);
            context.Builder.CreateCall(method, [.. args]);
            return true;
        }

        if (node.Callee.Name == "println")
        {
            var method = context.Driver.Compilation.Module.Resolver.FindMethod("System.Console::WriteLine", [.. args]);
            context.Builder.CreateCall(method, [.. args]);
            return true;
        }

        return false;
    }

    private static bool CreateStaticExternalCall(BodyCompilation context, CallExpression node, IEnumerable<Value> args)
    {
        if (node.Parent is BinaryOperator { Operator: "'::", Left: BinaryOperator typeNode })
        {
            var typename = new QualifiedTypeName(typeNode);
            var type = context.Driver.Compilation.Module.Resolver.FindType(typename.FullName);

            if (type == null)
            {
                typename.AddError("Type not found");
                return true;
            }

            var candidates = GetStaticMethodCandidates(node, args, type);

            if (candidates.Length == 0)
            {
                node.AddError("No matching function found");
            }

            var method = candidates[0];
            context.Builder.CreateCall(method, [.. args]);
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
}