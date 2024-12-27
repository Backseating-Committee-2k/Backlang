using DistIL.AsmIO;
using DistIL.IR;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Literals;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation;

public static class Utils
{
    public static string ToPascalCase(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        return char.ToUpper(name[0]) + name[1..];
    }

    public static TypeDesc? GetTypeFromNode(TypeName node, TypeDef containingType)
    {
        var resolvedType = GetTypeFromNodeImpl(node, containingType);

        if (resolvedType == null)
        {
            node.AddError($"Type '{node}' not found");
        }

        return resolvedType ?? PrimType.Void;
    }

    private static readonly Dictionary<string, TypeDesc> Primities = new()
    {
        ["none"]  = PrimType.Void,
        ["bool"]  = PrimType.Bool,
        ["i8" ] = PrimType.Byte,
        ["i16" ] = PrimType.Int16,
        ["i32" ] = PrimType.Int32,
        ["i64" ] = PrimType.Int64,
        ["f32" ] = PrimType.Single,
        ["f64" ] = PrimType.Double,
    };

    private static TypeDesc? GetTypeFromNodeImpl(TypeName node, TypeDef containingType)
    {
        if (node is SimpleTypeName id)
        {
            if (Primities.TryGetValue(id.Name, out var prim))
            {
                return prim;
            }

            var type = containingType.Module.FindType(containingType.Namespace, id.Name);
            if (type != null)
            {
                return type;
            }

            if (containingType.Name == id.Name)
            {
                return containingType;
            }
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
        else if (node is PointerTypeName ptr)
        {
            var type = GetTypeFromNodeImpl(ptr.Type, containingType);
            if (type != null)
            {
                return ptr.Kind switch
                {
                    PointerKind.Transient => type.CreatePointer(),
                    PointerKind.Reference => type.CreateByref(),
                    _ => throw new InvalidOperationException("Invalid pointerkind")
                };
            }
        }

        return null;
    }

    public static TypeDefOrSpec? GetTypeFromNode(TypeName node, ModuleDef module)
    {
        if (node is NoTypeName)
        {
            return null;
        }

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

    public static Value CreateValue(AstNode valueNode)
    {
        return valueNode switch
        {
            LiteralNode literal => CreateLiteral(literal.Value),
            _ => throw new NotImplementedException()
        };
    }

    private static Value CreateLiteral(object literalValue)
    {
        return literalValue switch
        {
            bool b => ConstInt.Create(PrimType.Bool, b ? 1 : 0),
            byte by => ConstInt.CreateI(by),
            short s => ConstInt.CreateI(s),
            int i => ConstInt.CreateI(i),
            long l => ConstInt.CreateL(l),
            float f => ConstFloat.CreateD(f),
            double d => ConstFloat.CreateD(d),
            string str => ConstString.Create(str),
            char c => ConstInt.CreateI(c),
            null => ConstNull.Create(),
            _ => throw new NotImplementedException()
        };
    }
}