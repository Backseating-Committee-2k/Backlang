using Backlang.CodeAnalysis.AST;
using Backlang.CodeAnalysis.AST.Literals;
using Backlang.Codeanalysis.Core;
using Backlang.CodeAnalysis.Parsing.ParsePoints;
using Loyc;
using Loyc.Syntax;
using LiteralNode = Backlang.CodeAnalysis.AST.Literals.LiteralNode;

namespace Backlang.Codeanalysis.Parsing;

public sealed partial class Parser
{
    private readonly Dictionary<string, Symbol> _literals = new()
    {
        { "ub", CodeSymbols.UInt8 },
        { "us", CodeSymbols.UInt16 },
        { "u", CodeSymbols.UInt32 },
        { "ui", CodeSymbols.UInt32 },
        { "ul", CodeSymbols.UInt64 },
        { "b", CodeSymbols.Int8 },
        { "s", CodeSymbols.Int16 },
        { "l", CodeSymbols.Int64 },
        { "h", Symbols.Float16 },
        { "f", Symbols.Float32 },
        { "d", Symbols.Float64 }
    };

    public void AddError(LocalizableString message, SourceRange range)
    {
        Messages.Add(Message.Error(message, range));
    }

    public void AddError(LocalizableString message)
    {
        Messages.Add(Message.Error(message,
            new SourceRange(Document, Iterator.Current.Start, Iterator.Current.Text.Length)));
    }

    internal AstNode? ParsePrimary(ParsePoints? parsePoints = null)
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

        return new ArrayLiteral(elements).WithRange(startToken, Iterator.Prev);
    }

    private AstNode? InvokeExpressionParsePoint(ParsePoints parsePoints)
    {
        var token = Iterator.Current;
        var type = token.Type;

        if (parsePoints.TryGetValue(type, out var value))
        {
            Iterator.NextToken();

            return value(Iterator, this).WithRange(token, Iterator.Prev);
        }

        AddError(ErrorID.UnknownExpression);
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

        return new LiteralNode(result).WithRange(Iterator.Prev);
    }

    private AstNode ParseBooleanLiteral(bool value)
    {
        Iterator.NextToken();

        return new LiteralNode(value).WithRange(Iterator.Prev);
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

            result = new LiteralNode(value).WithRange(Iterator.Prev);
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
                result = new LiteralNode(value).WithRange(Iterator.Prev);
            }
        }

        if (Iterator.Current.Type == TokenType.Identifier)
        {
            if (_literals.TryGetValue(Iterator.Current.Text.ToLower(), out var value))
            {
                result = new LiteralNode(result).WithRange(Iterator.Prev, Iterator.Current);
            }
            else
            {
                AddError(new LocalizableString(ErrorID.UnknownLiteral, Iterator.Current.Text));

                result = null;
            }

            Iterator.NextToken();
        }

        if (Iterator.IsMatch(TokenType.LessThan))
        {
            Iterator.NextToken();

            var unit = Iterator.Match(TokenType.Identifier);

            result = SyntaxTree.Unit(result, unit.Text)
                .WithRange(token, Iterator.Prev);

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