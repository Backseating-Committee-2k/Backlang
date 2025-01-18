using DistIL.AsmIO;

namespace SocordiaC.Compilation.Scoping;

public abstract class ScopeItem
{
    public required string Name { get; init; }
    public abstract TypeDesc Type { get; }
}