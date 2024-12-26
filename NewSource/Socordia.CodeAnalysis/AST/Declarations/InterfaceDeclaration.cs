using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Declarations;

public class InterfaceDeclaration(string name, List<TypeName> inheritances, List<AstNode> members)
    : ClassDeclaration(name, null, inheritances, members);