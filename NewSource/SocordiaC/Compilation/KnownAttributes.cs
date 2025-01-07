using System.ComponentModel;
using System.Runtime.CompilerServices;
using DistIL.AsmIO;
using Socordia.Core.CompilerService;

namespace SocordiaC.Compilation;

public class KnownAttributes
{
    private readonly Dictionary<Type, CustomAttrib> _cache = new();
    private readonly ModuleResolver _resolver;

    public KnownAttributes(ModuleResolver resolver)
    {
        _resolver = resolver;

        GetAttribute<ReadOnlyAttribute>();
        GetAttribute<UnitAttribute>();
        GetAttribute<ExtensionAttribute>();
    }

    public CustomAttrib GetAttribute<T>()
        where T : Attribute
    {
        var type = typeof(T);
        if (_cache.TryGetValue(type, out var cachedAttrib))
        {
            return cachedAttrib;
        }

        var importedType = _resolver.Import(type);
        var ctor = importedType.FindMethod(".ctor");
        var customAttrib = new CustomAttrib(ctor);

        _cache[type] = customAttrib;
        return customAttrib;
    }
}