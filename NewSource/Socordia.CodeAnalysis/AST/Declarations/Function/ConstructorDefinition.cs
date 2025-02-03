namespace Socordia.CodeAnalysis.AST.Declarations;

public class ConstructorDefinition(Signature signature, bool isExpressionBody, Block? body)
    : FunctionDefinition(signature, isExpressionBody, body);