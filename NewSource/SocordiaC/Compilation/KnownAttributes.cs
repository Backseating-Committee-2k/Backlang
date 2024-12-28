using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DistIL.AsmIO;
using Socordia.Core.CompilerService;

namespace SocordiaC.Compilation;

public class KnownAttributes
{
    public readonly TypeDefOrSpec MeasureAttribute;
    public readonly MethodDesc? MeasureAttributeCtor;

    public readonly TypeDefOrSpec ExtensionAttribute;
    public readonly MethodDesc? ExtensionAttributeCtor;

    public readonly TypeDefOrSpec UnitAttribute;
    public readonly MethodDesc? UnitAttributeCtor;


    public KnownAttributes(ModuleResolver resolver)
    {
        MeasureAttribute = resolver.Import(typeof(MeasureAttribute));
        MeasureAttributeCtor = MeasureAttribute.FindMethod(".ctor");

        ExtensionAttribute = resolver.Import(typeof(ExtensionAttribute));
        ExtensionAttributeCtor = ExtensionAttribute.FindMethod(".ctor");

        UnitAttribute = resolver.Import(typeof(UnitAttribute));
        UnitAttributeCtor = UnitAttribute.FindMethod(".ctor");
    }
}