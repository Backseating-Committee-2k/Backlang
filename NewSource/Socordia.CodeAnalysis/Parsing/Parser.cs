using System.Net.Mime;
using Loyc.Syntax;
using MrKWatkins.Ast.Position;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.Core;

namespace Socordia.CodeAnalysis.Parsing;

public sealed partial class Parser
{
    public readonly List<Message> Messages;

    public Parser(TextFile document, List<Token> tokens, List<Message> messages)
    {
        Document = document;
        Iterator = new TokenIterator(tokens, document);
        Messages = messages;

        InitParsePoints();
    }

    public TextFile Document { get; }

    public TokenIterator Iterator { get; set; }

    public static CompilationUnit Parse(TextFile src)
    {
        if (src.Text == null)
        {
            return new CompilationUnit
            {
                Declarations = new([]),
                Messages = [Message.Error("The source file is empty")],
                Document = src
            };
        }

        var lexer = new Lexer();
        var tokens = lexer.Tokenize(src);

        var parser = new Parser(src, tokens, lexer.Messages);

        return parser.Program();
    }

    public CompilationUnit Program()
    {
        var node = Start();

        Iterator.Match(TokenType.EOF);

        return node;
    }

    private CompilationUnit Start()
    {
        var cu = new CompilationUnit();

        var body = InvokeDeclarationParsePoints();

        cu.Messages = Messages.Concat(Iterator.Messages).ToList();
        cu.Declarations = new RootBlock(body);
        cu.Document = Document;

        return cu;
    }
}