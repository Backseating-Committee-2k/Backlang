using MrKWatkins.Ast.Position;
using Socordia.CodeAnalysis.Parsing;

namespace Socordia.CodeAnalysis.Core;

public abstract class BaseLexer
{
    public readonly List<Message> Messages = [];
    protected int _column = 1;
    protected TextFile _document;
    protected int _line = 1;
    protected int _position;

    public List<Token> Tokenize(TextFile document)
    {
        _document = document;

        var tokens = new List<Token>();

        Token newToken;
        do
        {
            newToken = NextToken();

            tokens.Add(newToken);
        } while (newToken.Type != TokenType.EOF);

        return tokens;
    }

    protected int Advance()
    {
        return ++_position;
    }

    protected char Current()
    {
        if (_position >= _document.Text.Length)
        {
            return '\0';
        }

        return _document.Text[_position];
    }

    protected abstract Token NextToken();

    protected char Peek(int offset = 0)
    {
        if (_position + offset >= _document.Text.Length)
        {
            return '\0';
        }

        return _document.Text[_position + offset];
    }

    protected void ReportError()
    {
        _column++;

        var pos = _document.CreatePosition(_position, 1, _line, _column);
        Messages.Add(Message.Error($"Unknown Character '{Current()}'", pos));
        Advance();
    }
}