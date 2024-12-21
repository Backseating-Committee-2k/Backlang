using Socordia.CodeAnalysis.AST;

namespace SocordiaC.Core.Semantic;

public interface ISemanticCheck
{
    void Check(CompilationUnit tree, Driver context);
}