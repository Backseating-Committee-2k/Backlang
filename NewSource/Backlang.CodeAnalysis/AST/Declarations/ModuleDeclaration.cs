namespace Backlang.CodeAnalysis.AST.Declarations;

public class ModuleDeclaration : AstNode
{
    public ModuleDeclaration(AstNode name)
    {
        Children.Add(name);
    }

    public AstNode Name => Children[0];
}