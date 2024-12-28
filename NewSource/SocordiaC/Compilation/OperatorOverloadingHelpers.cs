using System.Collections.Immutable;
using System.Reflection;
using DistIL.AsmIO;

namespace SocordiaC.Compilation;

public static class OperatorOverloadingHelpers
{
    private static readonly ImmutableDictionary<string, string> binMap = new Dictionary<string, string>
    {
        ["'+"] = "op_Addition",
        ["'/"] = "op_Division",
        ["'-"] = "op_Subtraction",
        ["'*"] = "op_Multiply",
        ["'%"] = "op_Modulus",
        ["'&"] = "op_BitwiseAnd",
        ["'|"] = "op_BitwiseOr",
        ["'^"] = "op_ExclusiveOr",
        ["'=="] = "op_Equality",
        ["'!="] = "op_Inequality"
    }.ToImmutableDictionary();

    private static readonly ImmutableDictionary<string, string> unMap = new Dictionary<string, string>
    {
        ["'!"] = "op_LogicalNot",
        ["'-"] = "op_UnaryNegation",
        ["'~"] = "op_OnesComplement",
        ["'*"] = "op_Deref",
        ["'&"] = "op_AddressOf",
        ["'%"] = "op_Percentage",
        ["'suf?"] = "op_Unpacking",
        ["implicit"] = "op_Implicit",
        ["explicit"] = "op_Explicit",
        ["default"] = "op_Default"
    }.ToImmutableDictionary();

    public static bool TryGetOperator(this TypeDesc type, string op, out MethodDef? opMethod, params TypeDefOrSpec[] args)
    {
        var possibleMethods = type.Methods.Cast<MethodDef>()
            .Where(_ => _ is { IsStatic: true, IsConstructor: false, IsDestructor: false, IsPublic: true }
                                                      && _.Attribs.HasFlag(MethodAttributes.SpecialName)
                                                      && _.Params.Length == args.Length
        );

        var nameMap = args.Length switch
        {
            1 => unMap,
            2 => binMap,
            _ => throw new Exception("Invalid number of arguments for operator")
        };

        possibleMethods =
            possibleMethods.Where(m => nameMap.ContainsValue(m.Name.ToString()) &&
                                       nameMap[op] == m.Name.ToString());

        foreach (var method in possibleMethods)
        {
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var param = method.Params[i].Type;

                if (arg != param)
                {
                    goto nextMethod;
                }
            }

            opMethod = method;
            return true;

            nextMethod: ;
        }

        opMethod = null;
        return false;
    }
}