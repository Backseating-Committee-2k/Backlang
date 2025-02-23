using Backlang.Codeanalysis.Core;
using Backlang.Codeanalysis.Parsing.AST;
using Loyc;
using Loyc.Syntax;

namespace Backlang.Codeanalysis.Parsing;

public sealed partial class Parser
{
    private readonly Dictionary<string, Symbol> _lits = new()
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

    internal LNode ParsePrimary(ParsePoints parsePoints = null)
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

    private LNode ParseArrayLiteral()
    {
        var startToken = Iterator.Current;
        Iterator.NextToken();

        var elements = Expression.ParseList(this, TokenType.CloseSquare);

        return SyntaxTree.Factory.Call(CodeSymbols.Array, elements).WithRange(startToken, Iterator.Prev);
    }

    private LNode Invalid(LocalizableString message)
    {
        AddError(message);

        return LNode.Call(CodeSymbols.Error, LNode.List(LNode.Literal(message)));
    }

    private LNode InvokeExpressionParsePoint(ParsePoints parsePoints)
    {
        var token = Iterator.Current;
        var type = token.Type;

        if (parsePoints.TryGetValue(type, out var value))
        {
            Iterator.NextToken();

            return value(Iterator, this).WithRange(token, Iterator.Prev);
        }

        if (type == Token.Invalid.Type)
        {
            return LNode.Missing;
        }

        return Invalid(ErrorID.UnknownExpression);
    }

    private LNode ParseBinNumber()
    {
        var valueToken = (UString)Iterator.NextToken().Text;

        var success = ParseHelpers.TryParseUInt(ref valueToken, out ulong result, 2, ParseNumberFlag.SkipUnderscores);

        if (!success)
        {
            return LNode.Missing;
        }

        return SyntaxTree.Factory.Call(CodeSymbols.Int32,
                LNode.List(SyntaxTree.Factory.Literal(result).WithStyle(NodeStyle.BinaryLiteral)))
            .WithRange(Iterator.Prev);
    }

    private LNode ParseBooleanLiteral(bool value)
    {
        Iterator.NextToken();

        return SyntaxTree.Factory.Call(CodeSymbols.Bool, LNode.List(SyntaxTree.Factory.Literal(value)))
            .WithRange(Iterator.Prev);
    }

    private LNode ParseChar()
    {
        var text = Iterator.NextToken().Text;
        var unescaped = ParseHelpers.UnescapeCStyle(text);

        return SyntaxTree.Factory.Call(CodeSymbols.Char,
            LNode.List(SyntaxTree.Factory.Literal(unescaped[0]))).WithRange(Iterator.Prev);
    }

    private LNode ParseHexNumber()
    {
        var valueToken = Iterator.NextToken();

        var parseSuccess = ParseHelpers.TryParseHex(valueToken.Text, out var result);

        if (!parseSuccess)
        {
            return LNode.Missing;
        }

        return SyntaxTree.Factory.Call(CodeSymbols.Int32,
                LNode.List(SyntaxTree.Factory.Literal(result)))
            .WithRange(Iterator.Prev);
    }

    private LNode ParseNumber()
    {
        var text = (UString)Iterator.NextToken().Text;

        LNode result;
        if (text.Contains('.'))
        {
            var value = ParseHelpers.TryParseDouble(ref text, 10, ParseNumberFlag.SkipUnderscores);

            result = SyntaxTree.Factory.Literal(value).WithRange(Iterator.Prev);
        }
        else
        {
            var success = ParseHelpers.TryParseInt(ref text, out int value, 10, ParseNumberFlag.SkipUnderscores);

            if (!success)
            {
                result = LNode.Missing;
            }
            else
            {
                result = SyntaxTree.Factory.Literal(value).WithRange(Iterator.Prev);
            }
        }

        if (Iterator.Current.Type == TokenType.Identifier)
        {
            if (_lits.TryGetValue(Iterator.Current.Text.ToLower(), out var value))
            {
                result = SyntaxTree.Factory.Call(value,
                    LNode.List(result)).WithRange(Iterator.Prev, Iterator.Current);
            }
            else
            {
                AddError(new LocalizableString(ErrorID.UnknownLiteral, Iterator.Current.Text));

                result = LNode.Missing;
            }

            Iterator.NextToken();
        }
        else if (result.Value is double)
        {
            result = SyntaxTree.Factory.Call(Symbols.Float64, LNode.List(result)).WithRange(result.Range);
        }
        else
        {
            result = SyntaxTree.Factory.Call(CodeSymbols.Int32, LNode.List(result)).WithRange(result.Range);
        }

        if (Iterator.IsMatch(TokenType.LessThan))
        {
            Iterator.NextToken();

            var unit = Iterator.Match(TokenType.Identifier);

            result = SyntaxTree.Unit(result, unit.Text).WithRange(result.Range.StartIndex, unit.End);

            Iterator.Match(TokenType.GreaterThan);
        }

        return result;
    }

    private LNode ParseString()
    {
        var valueToken = Iterator.NextToken();

        var unescaped = ParseHelpers.UnescapeCStyle(valueToken.Text);

        return SyntaxTree.Factory.Call(CodeSymbols.String, LNode.List(SyntaxTree.Factory.Literal(unescaped)))
            .WithRange(valueToken);
    }
}