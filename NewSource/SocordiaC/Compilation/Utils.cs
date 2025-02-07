using DistIL.AsmIO;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Literals;
using Socordia.CodeAnalysis.AST.TypeNames;
using SocordiaC.Compilation.Scoping;
using System.Reflection;
using System.Text;

namespace SocordiaC.Compilation;

public static partial class Utils
{
    private static readonly Dictionary<string, TypeDesc> Primities = new()
    {
        ["none"] = PrimType.Void,
        ["obj"] = PrimType.Object,
        ["bool"] = PrimType.Bool,
        ["string"] = PrimType.String,
        ["char"] = PrimType.Char,
        ["i8"] = PrimType.Byte,
        ["i16"] = PrimType.Int16,
        ["i32"] = PrimType.Int32,
        ["i64"] = PrimType.Int64,
        ["f32"] = PrimType.Single,
        ["f64"] = PrimType.Double
    };

    public static TypeDesc? GetTypeFromNode(TypeName node, TypeDef containingType)
    {
        var resolvedType = GetTypeFromNodeImpl(node, containingType);

        if (resolvedType == null)
        {
            node.AddError($"Type '{node}' not found");
        }

        return resolvedType ?? PrimType.Void;
    }

    private static TypeDesc? GetTypeFromNodeImpl(TypeName node, TypeDef containingType)
    {
        if (node is UnitTypeName unitTypeName)
        {
            var unitType = (TypeDef)GetTypeFromNodeImpl(unitTypeName.Unit, containingType);
            if (!unitType.IsUnitType())
            {
                unitTypeName.AddError("Type is not a unit type");
                return null;
            }

            return unitType;
        }

        if (node is SimpleTypeName id)
        {
            if (((Scope)node.Root.Tag!).TryGetTypeAlias(id.Name, out var alias))
            {
                return GetTypeFromNodeImpl(alias!, containingType);
            }

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

    public static TypeDesc? GetTypeFromNode(TypeName node, ModuleDef module)
    {
        if (node is NoTypeName)
        {
            return null;
        }

        if (node is SimpleTypeName id)
        {
            if (Primities.TryGetValue(id.Name, out var prim))
            {
                return prim;
            }
        }

        if (node is UnitTypeName unitTypeName)
        {
            return GetTypeFromNode(unitTypeName.Unit, module);
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

        throw new Exception($"cannot get type {node} from node");
    }

    public static TypeAttributes GetTypeModifiers(Declaration node)
    {
        var attrs = TypeAttributes.Public | TypeAttributes.BeforeFieldInit;

        foreach (var modifier in node.Modifiers)
        {
            attrs |= modifier switch
            {
                Modifier.Static => TypeAttributes.Sealed | TypeAttributes.Abstract,
                Modifier.Internal => TypeAttributes.NotPublic,
                Modifier.Public => TypeAttributes.Public,
                _ => throw new NotImplementedException()
            };
        }

        if (node.Modifiers.Contains(Modifier.Private) || node.Modifiers.Contains(Modifier.Internal))
        {
            attrs &= ~TypeAttributes.Public;
        }

        return attrs;
    }

    public static object? GetLiteralValue(AstNode? node)
    {
        if (node is LiteralNode lit)
        {
            return lit.Value;
        }

        return null;
    }

    public static bool IsUnitType(this TypeDef type)
    {
        var attribs = type.GetCustomAttribs();

        return attribs.Any(a => a.Type is { Namespace: "Socordia.Core.CompilerService", Name: "MeasureAttribute" });
    }

    public static FieldAttributes GetFieldModifiers(ParameterDeclaration parameter)
    {
        return FieldAttributes.Public | FieldAttributes.InitOnly;
    }

    public static void EmitAnnotation(Annotation annotation, ModuleEntity entity)
    {
        AppendAttributeToName();

        var type = GetTypeFromNode(annotation.Name, entity.Module);
        var ctor = type.FindMethod(".ctor");

        var attrib = new CustomAttrib(ctor);
        entity.GetCustomAttribs(false).Add(attrib);
        return;

        void AppendAttributeToName()
        {
            var name = annotation.Name;
            if (name.Children.Last is SimpleTypeName s && !s.Name.EndsWith("Attribute"))
            {
                s.Name += "Attribute";
            }
        }
    }

    public static void EmitAnnotations(Declaration declaration, ModuleEntity entity)
    {
        foreach (var annotation in declaration.Annotations)
        {
            EmitAnnotation(annotation, entity);
        }
    }

    public static string GenerateIdentifier()
    {
        var sb = new StringBuilder();
        sb.Append("<tmp>_");

        const string ALPHABET = "abcdefhijklmnopqrstABCDEFGHIJKLMNOPQRSTUVWXYZ&%$";
        var random = new Random();

        for (var i = 0; i < random.Next(5, 9); i++)
        {
            sb.Append(ALPHABET[random.Next(ALPHABET.Length)]);
        }

        return sb.ToString();
    }
}