using DistIL.AsmIO;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation;

public static class Utils
{
    public static TypeDesc? GetTypeFromNode(TypeName node, TypeDef containingType)
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

                return containingType.Module.Resolver.FindType(qname.ToString());
            }
        }

        throw new Exception("cannot get type from node");
    }

    public static TypeDefOrSpec? GetTypeFromNode(TypeName node, ModuleDef module)
    {
        if (node is QualifiedTypeName qname)
        {
            if (qname.Type is SimpleTypeName simple)
            {
                var type = module.FindType(qname.Namespace, simple.Name);
                if (type != null)
                {
                    return type;
                }

                return (TypeDef)module.Resolver.FindType(qname.ToString());
            }
        }

        throw new Exception("cannot get type from node");
    }
}