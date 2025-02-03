using Flo;
using Furesoft.Core.CodeDom.Compiler.Core.Constants;

namespace Backlang.Driver.Compiling.Stages.CompilationStages;

public sealed partial class ImplementationStage : IHandler<CompilerContext, CompilerContext>
{


    public static bool MatchesParameters(IMethod method, List<IType> argTypes)
    {
        //ToDo: fix matches parameter (implicit casting is currently not working)

        var matchesAllParameters = method.Parameters.Count == argTypes.Count;
        for (var i = 0; i < argTypes.Count; i++)
        {
            if (i == 0)
            {
                matchesAllParameters = argTypes[i].IsAssignableTo(method.Parameters[i].Type);
                continue;
            }

            matchesAllParameters &= argTypes[i].IsAssignableTo(method.Parameters[i].Type);
        }

        return matchesAllParameters;
    }

    public static IMethod GetMatchingMethod(CompilerContext context, List<IType> argTypes, IEnumerable<IMethod> methods,
        string methodname, bool shouldAppendError = true)
    {
        var candiates = new List<IMethod>();
        foreach (var m in methods.Where(_ => _.Name.ToString() == methodname))
        {
            if (m.Parameters.Count == argTypes.Count)
            {
                if (MatchesParameters(m, argTypes))
                {
                    candiates.Add(m);
                }
            }
        }

        if (shouldAppendError && candiates.Count == 0)
        {
            context.Messages.Add(Message.Error(
                $"Cannot find matching function '{methodname}({string.Join(", ", argTypes.Select(_ => _.FullName.ToString()))})'"));
            return null;
        }

        //ToDo: refactor getting best candidate
        var orderedCandidates = candiates.OrderByDescending(_ =>
            _.Parameters.Select(__ => _.FullName.ToString()).Contains("System.Object"));
        return orderedCandidates.FirstOrDefault();
    }
}