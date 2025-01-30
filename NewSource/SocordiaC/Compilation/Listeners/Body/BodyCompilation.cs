using DistIL.AsmIO;
using DistIL.IR.Utils;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using SocordiaC.Compilation.Scoping;

namespace SocordiaC.Compilation.Listeners.Body;

public record BodyCompilation(Driver Driver, MethodDef Method, IRBuilder Builder, Scope Scope)
{
    public object? Tag { get; set; }

    public static CompositeListener<BodyCompilation, AstNode> Listener =
        CompositeListener<BodyCompilation, AstNode>.Build()
            .With(new VariableDeclarationListener())
            .With(new CallExpressionListener(true))
            .With(new BinaryOperatorListener())
            .With(new ContinueListener())
            .With(new ThrowStatementListener())
            .With(new ReturnStatementListener())
            .With(new WhileStatementListener())
            .With(new IfStatementListener())
            .ToListener();
}