using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Core;
using Backlang.CodeAnalysis.Parsing.ParsePoints;
using Backlang.CodeAnalysis.Parsing.ParsePoints.Declarations;
using Backlang.CodeAnalysis.Parsing.ParsePoints.Expressions;
using Backlang.CodeAnalysis.Parsing.ParsePoints.Expressions.Match;
using Backlang.CodeAnalysis.Parsing.ParsePoints.Statements;
using Backlang.CodeAnalysis.Parsing.ParsePoints.Statements.Loops;
using Loyc.Syntax;

namespace Backlang.Codeanalysis.Parsing;

public sealed partial class Parser
{
    public readonly ParsePoints DeclarationParsePoints = new();
    public readonly ParsePoints ExpressionParsePoints = new();
    public readonly ParsePoints StatementParsePoints = new();

    public void InitParsePoints()
    {
        AddDeclarationParsePoint<BitFieldDeclaration>(TokenType.Bitfield);
        AddDeclarationParsePoint<UnionDeclaration>(TokenType.Union);
        AddDeclarationParsePoint<ClassDeclaration>(TokenType.Class);
        AddDeclarationParsePoint<ConstructorDeclarationParser>(TokenType.Constructor);
        AddDeclarationParsePoint<DestructorDeclaration>(TokenType.Destructor);
        AddDeclarationParsePoint<DiscriminatedUnionDeclaration>(TokenType.Type);
        AddDeclarationParsePoint<EnumDeclaration>(TokenType.Enum);
        AddDeclarationParsePoint<FunctionDeclaration>(TokenType.Function);
        AddDeclarationParsePoint<MacroDeclaration>(TokenType.Macro);
        AddDeclarationParsePoint<InterfaceDeclaration>(TokenType.Interface);
        AddDeclarationParsePoint<ImplementationDeclaration>(TokenType.Implement);
        AddDeclarationParsePoint<ImportStatementParser>(TokenType.Import);
        AddDeclarationParsePoint<StructDeclaration>(TokenType.Struct);
        AddDeclarationParsePoint<ModuleDeclarationParser>(TokenType.Module);
        AddDeclarationParsePoint<TypeAliasDeclaration>(TokenType.Using);
        AddDeclarationParsePoint<UnitDeclarationParser>(TokenType.Unit);
        AddDeclarationParsePoint<MacroBlockDeclaration>(TokenType.Identifier);

        AddExpressionParsePoint<IdentifierParser>(TokenType.Identifier);
        AddExpressionParsePoint<GroupOrTupleExpressionParser>(TokenType.OpenParen);
        AddExpressionParsePoint<MatchExpression>(TokenType.Match);
        AddExpressionParsePoint<DefaultExpressionParser>(TokenType.Default);
        AddExpressionParsePoint<SizeOfExpressionParser>(TokenType.SizeOf);
        AddExpressionParsePoint<TypeOfExpressionParser>(TokenType.TypeOf);
        AddExpressionParsePoint<NoneExpressionParser>(TokenType.None);
        AddExpressionParsePoint<InitializerListExpression>(TokenType.OpenSquare);

        AddStatementParsePoint<ThrowStatementParser>(TokenType.Throw);
        AddStatementParsePoint<BreakStatementParser>(TokenType.Break);
        AddStatementParsePoint<ContinueStatementParser>(TokenType.Continue);
        AddStatementParsePoint<ReturnStatementParser>(TokenType.Return);
        AddStatementParsePoint<VariableStatementParser>(TokenType.Let);
        AddStatementParsePoint<SwitchStatement>(TokenType.Switch);
        AddStatementParsePoint<IfStatementParser>(TokenType.If);
        AddStatementParsePoint<WhileStatementParser>(TokenType.While);
        AddStatementParsePoint<DoWhileStatementParser>(TokenType.Do);
        AddStatementParsePoint<TryStatementParser>(TokenType.Try);
        AddStatementParsePoint<ForStatement>(TokenType.For);
        AddStatementParsePoint<MacroBlockStatement>(TokenType.Identifier);
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

    public List<Declaration> InvokeDeclarationParsePoints(TokenType terminator = TokenType.EOF, ParsePoints parsePoints = null)
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

    public AstNode? InvokeParsePoint(ParsePoints parsePoints)
    {
        var type = Iterator.Current.Type;

        if (parsePoints.ContainsKey(type))
        {
            Iterator.NextToken();

            return parsePoints[type](Iterator, this);
        }

        var range = new SourceRange(Document, Iterator.Current.Start, Iterator.Current.Text.Length);

        AddError(new LocalizableString(ErrorID.UnknownExpression, Iterator.Current.Text), range);

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