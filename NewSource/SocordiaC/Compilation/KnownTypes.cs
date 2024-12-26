using DistIL.AsmIO;

namespace SocordiaC.Compilation;

public class KnownTypes
{
    public KnownTypes(ModuleResolver resolver)
    {
        ConsoleType = resolver.Import(typeof(Console));
    }

    public TypeDefOrSpec? ConsoleType { get; set; }
}