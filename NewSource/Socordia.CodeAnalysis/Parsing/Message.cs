using MrKWatkins.Ast.Position;

namespace Socordia.CodeAnalysis.Parsing;

public enum MessageSeverity
{
    Error, Warning, Info, Hint
}

public sealed class Message(MessageSeverity severity, string text, TextFilePosition range)
{
    public TextFilePosition Range { get; set; } = range;

    public TextFile Document { get; } = range.File;

    public MessageSeverity Severity { get; set; } = severity;
    public string Text { get; set; } = text;

    public static Message Error(string message, TextFilePosition range)
    {
        return new Message(MessageSeverity.Error, message, range);
    }

    public static Message Error(string message)
    {
        return new Message(MessageSeverity.Error, message, (TextFilePosition)TextFilePosition.None);
    }

    public static Message Info(string message, TextFilePosition range)
    {
        return new Message(MessageSeverity.Info, message, range);
    }

    public static Message Warning(string message, TextFilePosition range)
    {
        return new Message(MessageSeverity.Warning, message, range);
    }

    public override string ToString()
    {
        if (Document == null)
        {
            return Text;
        }

        return $"{Document.Name}:{Range.StartLine}:{Range.StartColumnNumber} {Text}";
    }
}