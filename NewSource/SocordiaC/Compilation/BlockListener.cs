using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;

namespace SocordiaC.Compilation;

public class BlockListener : Listener<Driver, AstNode, Block>
{
    protected override bool ShouldListenToChildren(Driver context, AstNode node) => true;
}