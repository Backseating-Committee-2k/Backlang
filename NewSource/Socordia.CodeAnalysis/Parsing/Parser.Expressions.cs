using DistIL.AsmIO;
using Loyc;
using Loyc.Syntax;
using MrKWatkins.Ast.Position;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Literals;
using Socordia.CodeAnalysis.Core;
using Socordia.CodeAnalysis.Parsing.ParsePoints;
using LiteralNode = Socordia.CodeAnalysis.AST.Literals.LiteralNode;

namespace Socordia.CodeAnalysis.Parsing;

public sealed partial class Parser
{
    private readonly Dictionary<string, PrimType> _literals = new()
    {
        { "ub", PrimType.Byte },
        { "us", PrimType.UInt16 },
        { "u", PrimType.UInt32 },
        { "ui", PrimType.UInt32 },
        { "ul", PrimType.UInt64 },
        { "b", PrimType.SByte },
        { "s", PrimType.Int16 },
        { "l", PrimType.Int64 },
        { "f", PrimType.Single },
        { "d", PrimType.Double }
    };

    public void AddError(string message, TextFilePosition range)
    {
        Messages.Add(Message.Error(message, range));
    }

    public void AddError(string message)
    {
        Messages.Add(Message.Error(message,
           Document.CreatePosition(Iterator.Position, Iterator.Current.Text.Length,
               Iterator.Current.Line, Iterator.Current.Column)));
    }

    internal AstNode? ParsePrimary(ParsePointCollection? parsePoints = null)
    {
        parsePoints ??= ExpressionParsePoints;

        return Iterator.Current.Type switch
        {
            TokenType.StringLiteral => ParseString(),
            TokenType.CharLiteral => ParseChar(),
            TokenType.Number => ParseNumber(),
            TokenType.HexNumber => ParseHexNumber(),
            TokenType.BinNumber => ParseBinNumber(),
            TokenType.TrueLiteral => ParseBooleanLiteral(true),
            TokenType.FalseLiteral => ParseBooleanLiteral(false),
            TokenType.OpenSquare => ParseArrayLiteral(),
            _ => InvokeExpressionParsePoint(parsePoints)
        };
    }

    private AstNode ParseArrayLiteral()
    {
        var startToken = Iterator.Current;
        Iterator.NextToken();

        var elements = Expression.ParseList(this, TokenType.CloseSquare);

        return new ArrayLiteral(elements);
    }

    private AstNode? InvokeExpressionParsePoint(ParsePointCollection parsePoints)
    {
        var token = Iterator.Current;
        var type = token.Type;

        if (parsePoints.TryGetValue(type, out var value))
        {
            Iterator.NextToken();

            return value(Iterator, this);
        }

        AddError("Unexpected Expression " + token);
        return null;
    }

    private AstNode? ParseBinNumber()
    {
        var valueToken = (UString)Iterator.NextToken().Text;

        var success = ParseHelpers.TryParseUInt(ref valueToken, out ulong result, 2, ParseNumberFlag.SkipUnderscores);

        if (!success)
        {
            return null;
        }

        return new LiteralNode(result);
    }

    private AstNode ParseBooleanLiteral(bool value)
    {
        Iterator.NextToken();

        return new LiteralNode(value);
    }

    private AstNode ParseChar()
    {
        var text = Iterator.NextToken().Text;
        var unescaped = ParseHelpers.UnescapeCStyle(text);

        return new LiteralNode(unescaped);
    }

    private AstNode? ParseHexNumber()
    {
        var valueToken = Iterator.NextToken();

        var parseSuccess = ParseHelpers.TryParseHex(valueToken.Text, out var result);

        if (!parseSuccess)
        {
            return null;
        }

        return new LiteralNode(result);
    }

    private AstNode ParseNumber()
    {
        var token = Iterator.NextToken();
        var text = (UString)token.Text;

        AstNode? result;
        if (text.Contains('.'))
        {
            var value = ParseHelpers.TryParseDouble(ref text, 10, ParseNumberFlag.SkipUnderscores);

            result = new LiteralNode(value);
        }
        else
        {
            var success = ParseHelpers.TryParseInt(ref text, out int value, 10, ParseNumberFlag.SkipUnderscores);

            if (!success)
            {
                result = null;
            }
            else
            {
                result = new LiteralNode(value);
            }
        }

        if (Iterator.Current.Type == TokenType.Identifier)
        {
            if (_literals.TryGetValue(Iterator.Current.Text.ToLower(), out var value))
            {
                result = new LiteralNode(result);
            }
            else
            {
                AddError("Unknown Literal "+ Iterator.Current.Text);

                result = null;
            }

            Iterator.NextToken();
        }

        if (Iterator.IsMatch(TokenType.LessThan))
        {
            Iterator.NextToken();

            var unit = Iterator.Match(TokenType.Identifier);

            result = SyntaxTree.Unit(result, unit.Text);

            Iterator.Match(TokenType.GreaterThan);
        }

        return result;
    }

    private AstNode ParseString()
    {
        var valueToken = Iterator.NextToken();

        var unescaped = ParseHelpers.UnescapeCStyle(valueToken.Text);

        return new LiteralNode(unescaped);
    }
}