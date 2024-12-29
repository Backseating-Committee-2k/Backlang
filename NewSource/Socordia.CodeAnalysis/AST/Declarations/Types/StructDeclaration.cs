using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Declarations;

public class StructDeclaration(
    string name,
    TypeName? baseType,
    List<TypeName> inheritances,
    List<TypeMemberDeclaration> members)
    : ClassDeclaration(name, baseType, inheritances, members);