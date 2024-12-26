using DistIL.AsmIO;
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

        if (node.Parent is BinaryOperator { Operator: "'::", Left: BinaryOperator typeNode })
        {
            var typename = new QualifiedTypeName(typeNode);
            var type = context.Driver.Compilation.Module.Resolver.FindType(typename.FullName);

            if (type == null)
            {
                typename.AddError("Type not found");
                return;
            }

            var candidates = type.Methods
                .Where(m => m.Name == node.Callee.Name)
                .Where(m => m.ParamSig.Count == node.Arguments.Count())
                .Where(m => m.ParamSig.Zip(
                    args,
                    (p, a) => p.Type.IsAssignableTo(a.ResultType)).All(x => x)).ToArray();

            if (candidates.Length == 0)
            {
                node.AddError("No matching function found");
            }

            var method = candidates[0];
            context.Builder.CreateCall(method, [.. args]);
            return;
        }

        if (node.Callee.Name == "print")
        {
            var method = context.Driver.Compilation.Module.Resolver.FindMethod("System.Console::Write", [.. args]);
            context.Builder.CreateCall(method, [.. args]);
            return;
        }

        if (node.Callee.Name == "println")
        {
            var method = context.Driver.Compilation.Module.Resolver.FindMethod("System.Console::WriteLine", [.. args]);
            context.Builder.CreateCall(method, [.. args]);
            return;
        }

        node.AddError("Function not found");
    }
}