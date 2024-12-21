using System.Xml;

namespace Backlang.CodeAnalysis.AST;

public class DocComment : AstNode
{
    public DocComment(XmlDocument document)
    {
        Properties.Set(nameof(Document), document);
    }

    public XmlDocument Document => Properties.GetOrThrow<XmlDocument>(nameof(Document));
}