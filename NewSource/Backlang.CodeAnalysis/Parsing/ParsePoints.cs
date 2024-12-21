using Backlang.CodeAnalysis.AST;

namespace Backlang.Codeanalysis.Parsing;

public sealed class ParsePoints : Dictionary<TokenType, Func<TokenIterator, Parser, AstNode>>
{
}