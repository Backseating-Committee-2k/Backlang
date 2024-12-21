using System.Xml;
using Backlang.CodeAnalysis.AST;
using Backlang.Codeanalysis.Parsing;
using Loyc;
using Loyc.Syntax;

namespace Backlang.CodeAnalysis.Parsing.ParsePoints;

public sealed class DocCommentParser
{
    public static DocComment Parse(Parser parser)
    {
        var comment = parser.Iterator.Match(TokenType.DocComment);
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml($"<root>{comment.Text}</root>");

        return new DocComment(xmlDocument);
    }

    public static bool TryParse(Parser parser, out DocComment? node)
    {
        var result = parser.Iterator.IsMatch(TokenType.DocComment);

        node = result ? Parse(parser) : null;
        return result;
    }
}