using DistIL.AsmIO;
using DistIL.IR.Utils;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using SocordiaC.Core.Scoping;

namespace SocordiaC.Compilation.Body;

public record BodyCompilation(Driver Driver, MethodDef Method, IRBuilder Builder, Scope Scope)
{
    public static CompositeListener<BodyCompilation, AstNode> Listener =
        CompositeListener<BodyCompilation, AstNode>.Build()
            .With(new VariableDeclarationListener())
            .With(new CallExpressionListener(true))
            .With(new BinaryOperatorListener())
            .With(new ReturnStatementListener())
            .ToListener();
}