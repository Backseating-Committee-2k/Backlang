namespace Socordia.CodeAnalysis.Parsing;

public record struct OperatorInfo(TokenType TokenType, int Precedence, bool IsUnary, bool IsPostUnary);