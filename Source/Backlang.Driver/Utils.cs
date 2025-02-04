using Backlang.Core.CompilerService;
using Furesoft.Core.CodeDom.Compiler.TypeSystem;
using System.Runtime.CompilerServices;

namespace Backlang.Driver;

public static class Utils
{
    

    public static void AddCompilerGeneratedAttribute(TypeResolver binder, DescribedType type)
    {
        var attributeType = ResolveType(binder, typeof(CompilerGeneratedAttribute));

        type.AddAttribute(new DescribedAttribute(attributeType));
    }
}