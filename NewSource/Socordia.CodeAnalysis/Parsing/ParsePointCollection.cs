using Socordia.CodeAnalysis.AST;

namespace Socordia.CodeAnalysis.Parsing;

public sealed class ParsePointCollection : Dictionary<TokenType, Func<TokenIterator, Parser, AstNode>>
{
}