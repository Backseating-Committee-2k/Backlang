﻿using Backlang.Codeanalysis.Parsing.AST;

namespace Backlang.Driver.Compiling;

public sealed class PassManager
{
    private readonly List<Func<CompilationUnit, CompilationUnit>> Passes = new();

    public void AddPass(Func<CompilationUnit, CompilationUnit> pass)
    {
        Passes.Add(pass);
    }

    public CompilationUnit Process(CompilationUnit compilationUnit)
    {
        var result = compilationUnit;
        for (var i = 0; i < Passes.Count; i++)
        {
            var pass = Passes[i];

            result = pass(result);
        }

        return result;
    }
}