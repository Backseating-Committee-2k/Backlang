using MrKWatkins.Ast;
using Socordia.CodeAnalysis.Parsing;

namespace Socordia.CodeAnalysis.AST;

public class AstNode : PropertyNode<AstNode>
{
    public AstNode WithRange(Token keywordToken, Token iteratorPrev)
    {
        throw new NotImplementedException();
    }

    public AstNode WithRange(Token token)
    {
        throw new NotImplementedException();
    }
}