using Backlang.CodeAnalysis.AST;

namespace Backlang.Codeanalysis.Parsing;

public interface IParsePoint
{
    static abstract AstNode Parse(TokenIterator iterator, Parser parser);
}