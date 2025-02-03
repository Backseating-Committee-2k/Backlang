using DistIL.AsmIO;

namespace SocordiaC.Compilation;

public static class ImplicitTypeCastTable
{
    private static readonly Dictionary<TypeDesc, TypeDesc[]> CastMap = new();

    static ImplicitTypeCastTable()
    {
        CastMap.Add(PrimType.SByte, [PrimType.UInt16, PrimType.UInt32, PrimType.UInt64]);
        CastMap.Add(PrimType.UInt16, [PrimType.UInt32, PrimType.UInt64]);
        CastMap.Add(PrimType.UInt32, [PrimType.UInt64]);

        CastMap.Add(PrimType.Byte, [PrimType.Int16, PrimType.Int32, PrimType.Int64, PrimType.Single]);
        CastMap.Add(PrimType.Int16, [PrimType.Int32, PrimType.Int64, PrimType.Single, PrimType.Double]);
        CastMap.Add(PrimType.Int32, [PrimType.Int64, PrimType.Double]);
    }

    public static bool IsAssignableTo(this TypeDef type, TypeDef toCast)
    {
        if (type == toCast) return true;

        /* if (toCast.IsUnitType())
         {
             return IsAssignableTo(type, ut.BaseTypes[0]);
         }*/

        if (HasImplicitCastOperator(type, toCast)) return true;

        if (CastMap.TryGetValue(toCast, out var value)) return value.Contains(type);

        return toCast == PrimType.Object;
    }

    private static bool HasImplicitCastOperator(TypeDesc type, TypeDesc toCast)
    {
        var result = type.TryGetOperator("implicit", out var method, type);

        return result && method?.ReturnType == toCast;
    }
}