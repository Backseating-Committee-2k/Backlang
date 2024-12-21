using Backlang.CodeAnalysis.AST;

namespace BacklangC.Core.Semantic;

public interface ISemanticCheck
{
    void Check(CompilationUnit tree, Driver context);
}