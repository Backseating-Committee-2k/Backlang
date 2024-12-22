using DistIL.AsmIO;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation;

public static class Utils
{
    public static TypeDesc? GetTypeFromNode(AstNode node, TypeDef containingType)
    {
        if (node is SimpleTypeName id)
        {
            return id.Name switch
            {
                "none" => PrimType.Void,
                "bool" => PrimType.Bool,
                "i8" => PrimType.Byte,
                "i16" => PrimType.Int16,
                "i32" => PrimType.Int32,
                "i64" => PrimType.Int64,
                "f32" => PrimType.Single,
                "f64" => PrimType.Double,
                var x => containingType.Name == x ? containingType : null
            };
        }
        else if (node is QualifiedTypeName qname)
        {
            if (qname.Type is SimpleTypeName simple)
            {
                var type = containingType.Module.FindType(qname.Namespace, simple.Name);
                if (type != null)
                {
                    return type;
                }

                //todo: check in all modules for type
                return null;
            }
        }


        throw new Exception("cannot get type from node");
    }
}