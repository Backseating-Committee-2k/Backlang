using DistIL.AsmIO;
using DistIL.IR.Utils;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;

namespace SocordiaC.Compilation.Body;

public record BodyCompilation(Driver Driver, MethodDef Method, IRBuilder Builder)
{
    public static CompositeListener<BodyCompilation, AstNode> Listener =
        CompositeListener<BodyCompilation, AstNode>.Build()
            .With(new VariableDeclarationListener())
            .With(new CallExpressionListener())
            .With(new ReturnStatementListener())
            .ToListener();
}