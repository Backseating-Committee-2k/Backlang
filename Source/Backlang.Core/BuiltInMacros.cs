﻿using LeMP;
using Loyc.Syntax;

namespace Backlang.Core;

[ContainsMacros]
public static class BuiltInMacros
{
    private static LNodeFactory F = new LNodeFactory(EmptySourceFile.Synthetic);

    [LexicalMacro(@"nameof(id_or_expr)",
        @"Converts the 'key' name component of an expression to a string (e.g. nameof(A.B<C>(D)) == ""B"")")]
    public static LNode @nameof(LNode nameof, IMacroContext context)
    {
        if (nameof.ArgCount != 1)
            return null;

        return nameof.Args[0]; //ToDo: need to be implemented
    }
}