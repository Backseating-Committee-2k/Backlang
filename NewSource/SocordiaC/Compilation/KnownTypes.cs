using DistIL.AsmIO;

namespace SocordiaC.Compilation;

public class KnownTypes(ModuleResolver resolver)
{
    public TypeDefOrSpec? ConsoleType { get; set; } = resolver.Import(typeof(Console));
}