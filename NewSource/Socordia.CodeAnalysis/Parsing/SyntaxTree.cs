using Loyc;
using Loyc.Syntax;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Expressions;
using Socordia.CodeAnalysis.AST.Literals;
using Socordia.CodeAnalysis.AST.Statements;
using Socordia.CodeAnalysis.AST.Statements.Loops;
using Socordia.CodeAnalysis.AST.TypeNames;
using Socordia.CodeAnalysis.Parsing.ParsePoints;
using LiteralNode = Socordia.CodeAnalysis.AST.Literals.LiteralNode;

namespace Socordia.CodeAnalysis.Parsing;

public static class SyntaxTree
{
    public static LNodeFactory Factory = new(EmptySourceFile.Unknown);

    public static LNode Annotation(LNode call)
    {
        return Factory.Call(Symbols.Annotation).PlusAttr(call);
    }

    public static LNode Constructor(LNodeList parameters, LNode code)
    {
        return Factory.Call(Symbols.Constructor, LNode.List(Factory.AltList(parameters), code));
    }

    public static LNode DiscriminatedType(Token nameToken, LNodeList parameters)
    {
        return Factory.Call(Symbols.DiscriminatedType,
            LNode.List(Factory.FromToken(nameToken), Factory.AltList(parameters)));
    }

    public static LNode DiscriminatedUnion(Token nameToken, LNodeList types)
    {
        return Factory.Call(Symbols.DiscriminatedUnion,
            LNode.List(Factory.FromToken(nameToken), Factory.AltList(types)));
    }

    public static LNode Property(LNode type, LNode name, LNode getter, LNode setter, LNode value)
    {
        if (value != null)
        {
            return LNode.Call(CodeSymbols.Property,
                LNode.List(type, getter, setter, LNode.Call(CodeSymbols.Assign, LNode.List(name, value))));
        }

        return LNode.Call(CodeSymbols.Property, LNode.List(type, getter, setter, name));
    }

    public static LNode Destructor(LNodeList parameters, LNode code)
    {
        return Factory.Call(Symbols.Destructor, LNode.List(Factory.AltList(parameters), code));
    }

    public static LNode Array(LNode typeNode, int dimensions)
    {
        return Factory.Call(CodeSymbols.Array, LNode.List(typeNode, LNode.Literal(dimensions)));
    }

  public static AstNode Throw(AstNode arg)
{
    return new ThrowStatement(arg);
}

    public static AstNode ArrayInstantiation(List<AstNode> elements)
    {
        return new CollectionExpression(elements);
    }

    public static AstNode ArrayInstantiation(AstNode arr, List<AstNode> indices)
    {
        return arr; //.WithArgs(indices);
    }

    public static AstNode Binary(Symbol op, AstNode left, AstNode right)
    {
        return new BinaryOperator(op, left, right);
    }

    public static LNode Bitfield(Token nameToken, LNodeList members)
    {
        return Factory.Call(Symbols.Bitfield, LNode.List(Factory.FromToken(nameToken), Factory.AltList(members)));
    }

    public static LNode Case(LNode condition, LNode body)
    {
        return Factory.Call(CodeSymbols.Case, LNode.List(condition, body));
    }

    public static CatchStatement Catch(Identifier exceptionType, Identifier exceptionValueName, Block body)
    {
        return new CatchStatement(exceptionType, exceptionValueName, body);
    }

    public static AstNode Class(Token nameToken, List<AstNode> inheritances, List<AstNode> members)
    {
        return new ClassDeclaration(nameToken.Text, inheritances, members);
    }

    public static AstNode Default(AstNode? type = null)
    {
        return new DefaultLiteral(type);
    }

    public static LNode Enum(LNode name, LNodeList members)
    {
        return Factory.Call(CodeSymbols.Enum, Factory.AltList(name,
            Factory.Call(CodeSymbols.AltList),
            Factory.Call(CodeSymbols.Braces,
                members)));
    }

    public static AstNode For(AstNode varExpr, AstNode type, AstNode arr, Block body)
    {
        return new ForStatement(varExpr, type, arr, body);
    }

    public static AstNode If(AstNode cond, Block ifBody, Block elseBody)
    {
        return new IfStatement(cond, ifBody, elseBody);
    }

    public static LNode ImplDecl(LNode target, LNodeList body)
    {
        var attributes = new LNodeList();

        return Factory.Call(Symbols.Implementation,
            Factory.AltList(target, LNode.Call(CodeSymbols.Braces,
                body).SetStyle(NodeStyle.StatementBlock))).WithAttrs(attributes);
    }

   public static AstNode Import(AstNode expr)
    {
        return new ImportStatement(expr);
    }

    public static LNode Interface(Token nameToken, LNodeList inheritances, LNodeList members)
    {
        return Factory.Call(CodeSymbols.Interface,
            LNode.List(
                Factory.FromToken(nameToken),
                LNode.Call(Symbols.Inheritance, inheritances),
                LNode.Call(CodeSymbols.Braces, members).SetStyle(NodeStyle.StatementBlock)));
    }

    public static AstNode Module(AstNode ns)
    {
        return new ModuleDeclaration(ns);
    }

    public static AstNode None()
    {
        return new LiteralNode(null);
    }

    public static LNode Pointer(LNode type)
    {
        return Factory.Call(Symbols.PointerType, LNode.List(type));
    }

    public static LNode RefType(LNode type)
    {
        return Factory.Call(Symbols.RefType, LNode.List(type));
    }

    public static LNode NullableType(LNode type)
    {
        return Factory.Call(Symbols.NullableType, LNode.List(type));
    }

    public static Signature Signature(AstNode name, AstNode type, List<ParameterDeclaration> parameters, List<AstNode> generics)
    {
        return new Signature(name, type, parameters, generics);
    }

    public static AstNode SizeOf(AstNode type)
    {
        return new SizeOf(type);
    }

    public static LNode Struct(Token nameToken, LNodeList inheritances, LNodeList members)
    {
        return Factory.Call(CodeSymbols.Struct,
            LNode.List(
                Factory.FromToken(nameToken),
                Factory.Call(Symbols.Inheritance, inheritances),
                Factory.Call(CodeSymbols.Braces, members).SetStyle(NodeStyle.StatementBlock)));
    }

    public static LNode Switch(LNode element, LNodeList cases)
    {
        return Factory.Call(CodeSymbols.SwitchStmt,
            LNode.List(element, LNode.Call(CodeSymbols.Braces, cases).SetStyle(NodeStyle.StatementBlock)));
    }

    public static AstNode Try(Block body, List<CatchStatement> catches, Block? finallly)
    {
        return new TryStatement(body, catches, finallly);
    }

    public static AstNode Unary(string op, AstNode operand, UnaryOperatorKind kind)
    {
        return new UnaryOperator(op, operand, kind);
    }

    public static LNode Union(string name, LNodeList members)
    {
        return Factory.Call(Symbols.Union, LNode.List(Factory.Id(name)).Add(Factory.AltList(members)));
    }

    public static LNode Using(LNode expr)
    {
        if (!expr.Calls(CodeSymbols.As)) // TODO: throw error in intermediate stage when has only one arg
        {
            return Factory.Call(CodeSymbols.UsingStmt, LNode.Missing);
        }

        return Factory.Call(CodeSymbols.UsingStmt, LNode.List(expr[0], expr[1]));
    }

    public static LNode When(LNode binOp, LNode rightHand, LNode body)
    {
        return Factory.Call(CodeSymbols.When, LNode.List(binOp, rightHand, body));
    }

    public static AstNode While(AstNode cond, Block body)
    {
        return new WhileStatement(cond, body);
    }

    public static AstNode Unit(AstNode value, string unit)
    {
        return new UnitLiteral(value, unit);
    }

    public static AstNode UnitDeclaration(Token nameToken)
    {
        return new UnitDeclaration(nameToken.Text);
    }

    public static AstNode TypeOfExpression(AstNode type)
    {
        return new TypeOf(type);
    }

    public static AstNode DoWhile(Block body, AstNode cond)
    {
        return new DoWhileStatement(body, cond);
    }

    public static AstNode Tuple(List<AstNode> exprs)
    {
        return new TupleLiteral(exprs);
    }

    public static AstNode Id(string name)
    {
        return new Identifier(name);
    }
}