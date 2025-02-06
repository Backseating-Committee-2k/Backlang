using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.Parsing.ParsePoints;
using Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;
using Socordia.CodeAnalysis.Parsing.ParsePoints.Expressions;
using Socordia.CodeAnalysis.Parsing.ParsePoints.Statements;
using Socordia.CodeAnalysis.Parsing.ParsePoints.Statements.Loops;

namespace Socordia.CodeAnalysis.Parsing;

public sealed partial class Parser
{
    public readonly ParsePointCollection DeclarationParsePoints = new();
    public readonly ParsePointCollection ExpressionParsePoints = new();
    public readonly ParsePointCollection StatementParsePoints = new();

    public void InitParsePoints()
    {
        AddDeclarationParsePoint<ImportStatementParser>(TokenType.Import);
        AddDeclarationParsePoint<ModuleDeclarationParser>(TokenType.Module);
        AddDeclarationParsePoint<TypeAliasDeclarationParser>(TokenType.Using);

        AddDeclarationParsePoint<EnumDeclarationParser>(TokenType.Enum);
        AddDeclarationParsePoint<StructDeclarationParser>(TokenType.Struct);
        AddDeclarationParsePoint<ClassDeclarationParser>(TokenType.Class);
        AddDeclarationParsePoint<InterfaceDeclarationParser>(TokenType.Interface);
        AddDeclarationParsePoint<DelegateDeclarationParser>(TokenType.Delegate);
        AddDeclarationParsePoint<UnionDeclarationParser>(TokenType.Union);
        AddDeclarationParsePoint<UnitDeclarationParser>(TokenType.Unit);
        AddDeclarationParsePoint<DiscriminatedUnionDeclarationParser>(TokenType.Type);

        AddDeclarationParsePoint<FunctionDefinitionParser>(TokenType.Function);
        AddDeclarationParsePoint<RulesForDeclarationParser>(TokenType.Rules);

        /*  AddDeclarationParsePoint<ConstructorDeclarationParser>(TokenType.Constructor);
          AddDeclarationParsePoint<DestructorDeclaration>(TokenType.Destructor);
          AddDeclarationParsePoint<BitFieldDeclaration>(TokenType.Bitfield);
          AddDeclarationParsePoint<MacroDeclaration>(TokenType.Macro);
          AddDeclarationParsePoint<ImplementationDeclaration>(TokenType.Implement);*/
        // AddDeclarationParsePoint<MacroBlockDeclaration>(TokenType.Identifier);

        AddExpressionParsePoint<IdentifierParser>(TokenType.Identifier);
        AddExpressionParsePoint<GroupOrTupleExpressionParser>(TokenType.OpenParen);
        AddExpressionParsePoint<DefaultExpressionParser>(TokenType.Default);
        AddExpressionParsePoint<SizeOfExpressionParser>(TokenType.SizeOf);
        AddExpressionParsePoint<TypeOfExpressionParser>(TokenType.TypeOf);
        AddExpressionParsePoint<NoneExpressionParser>(TokenType.None);
        AddExpressionParsePoint<InitializerListExpression>(TokenType.OpenSquare);
        // AddExpressionParsePoint<MatchExpression>(TokenType.Match);

        AddStatementParsePoint<ThrowStatementParser>(TokenType.Throw);
        AddStatementParsePoint<BreakStatementParser>(TokenType.Break);
        AddStatementParsePoint<ContinueStatementParser>(TokenType.Continue);
        AddStatementParsePoint<ReturnStatementParser>(TokenType.Return);
        AddStatementParsePoint<VariableStatementParser>(TokenType.Let);
        // AddStatementParsePoint<SwitchStatement>(TokenType.Switch);
        AddStatementParsePoint<IfStatementParser>(TokenType.If);
        AddStatementParsePoint<WhileStatementParser>(TokenType.While);
        AddStatementParsePoint<DoWhileStatementParser>(TokenType.Do);
        AddStatementParsePoint<TryStatementParser>(TokenType.Try);
        AddStatementParsePoint<ForStatement>(TokenType.For);
        //AddStatementParsePoint<MacroBlockStatement>(TokenType.Identifier);
    }

    public void AddDeclarationParsePoint<T>(TokenType type)
        where T : IParsePoint
    {
        DeclarationParsePoints.Add(type, T.Parse);
    }

    public void AddExpressionParsePoint<T>(TokenType type)
        where T : IParsePoint
    {
        ExpressionParsePoints.Add(type, T.Parse);
    }

    public void AddStatementParsePoint<T>(TokenType type)
        where T : IParsePoint
    {
        StatementParsePoints.Add(type, T.Parse);
    }

    public List<Declaration> InvokeDeclarationParsePoints(TokenType terminator = TokenType.EOF,
        ParsePointCollection parsePoints = null)
    {
        if (parsePoints == null)
        {
            parsePoints = DeclarationParsePoints;
        }

        var declarations = new List<Declaration>();
        while (Iterator.Current.Type != terminator)
        {
            _ = DocCommentParser.TryParse(this, out var docComment);
            _ = AnnotationParser.TryParse(this, out var annotations);
            _ = ModifierParser.TryParse(this, out var modifiers);

            var node = (Declaration?)InvokeParsePoint(parsePoints);

            node!.Annotations.AddRange(annotations);
            node.DocComment = docComment;
            node.Modifiers = modifiers;

            if (node != null)
            {
                declarations.Add(node);
            }
        }

        return declarations;
    }

    public AstNode? InvokeParsePoint(ParsePointCollection parsePoints)
    {
        var type = Iterator.Current.Type;
        var oldPos = Iterator.Position;

        if (parsePoints.ContainsKey(type))
        {
            Iterator.NextToken();

            return parsePoints[type](Iterator, this);
        }

        var pos = Document.CreatePosition(oldPos,
            Iterator.Current.Text.Length, Iterator.Current.Line, Iterator.Current.Column);

        AddError("Unexpected Expression " + Iterator.Current.Text, pos);

        Iterator.NextToken();

        return default;
    }

    public AstNode InvokeStatementParsePoint()
    {
        var type = Iterator.Current.Type;

        if (StatementParsePoints.ContainsKey(type))
        {
            Iterator.NextToken();

            return StatementParsePoints[type](Iterator, this);
        }

        return ExpressionStatementParser.Parse(Iterator, this);
    }
}