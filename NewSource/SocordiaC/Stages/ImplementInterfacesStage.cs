using Flo;
using SocordiaC.Compilation;

namespace SocordiaC.Stages;

public class ImplementInterfacesStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var (node, type) in Mappings.Types)
        foreach (var impl in node.Implementations)
        {
            var t = Utils.GetTypeFromNode(impl, type);

            if (t.IsInterface)
                type.Interfaces.Add(t);
            else
                impl.AddError(impl + " is not an interface");
        }

        Mappings.Types.Clear();

        return await next.Invoke(context);
    }
}