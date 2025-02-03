using DistIL.AsmIO;
using DistIL.IR;

namespace SocordiaC.Compilation.Scoping.Items;

public class ParameterScopeItem : ScopeItem
{
    public required Argument Arg { get; init; }

    public override TypeDesc Type => Arg.ResultType;


    public void Deconstruct(out string name, out Argument parameter, out TypeDesc type)
    {
        name = Name;
        parameter = Arg;
        type = Type;
    }
}