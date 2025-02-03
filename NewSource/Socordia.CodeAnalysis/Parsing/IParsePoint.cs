using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing;

public interface IParsePoint
{
    static abstract AstNode Parse(TokenIterator iterator, Parser parser);
}