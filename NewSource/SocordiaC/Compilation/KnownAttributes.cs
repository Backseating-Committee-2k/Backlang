using DistIL.AsmIO;

namespace SocordiaC.Compilation;

public class KnownAttributes(ModuleResolver resolver)
{
    private readonly Dictionary<Type, CustomAttrib> _cache = new();

    public CustomAttrib GetAttribute<T>()
        where T : Attribute
    {
        var type = typeof(T);
        if (_cache.TryGetValue(type, out var cachedAttrib))
        {
            return cachedAttrib;
        }

        var importedType = resolver.Import(type);
        var ctor = importedType.FindMethod(".ctor");
        var customAttrib = new CustomAttrib(ctor);

        _cache[type] = customAttrib;
        return customAttrib;
    }
}