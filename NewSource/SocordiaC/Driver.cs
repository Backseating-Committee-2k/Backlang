using System.Reflection;
using DistIL;
using DistIL.AsmIO;
using Flo;
using LanguageSdk.Templates.Core;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.Parsing;
using SocordiaC.Compilation;
using SocordiaC.Stages;
using System.Runtime.Versioning;

namespace SocordiaC;

public class Driver
{
    public DriverSettings Settings { get; private init; } = new();
    public required DistIL.Compilation Compilation { get; set; }

    public required KnownAttributes KnownAttributes { get; set; }
    public required KnownTypes KnownTypes { get; set; }

    private Dictionary<string, TypeDef> _functionTypes { get; } = new();

    public Optimizer Optimizer { get; set; }
    public List<Message> Messages { get; set; } = [];
    public List<CompilationUnit> Trees { get; set; } = [];

    public static Driver Create(DriverSettings settings)
    {
        var moduleResolver = new ModuleResolver();
        moduleResolver.AddTrustedSearchPaths();

        var module = moduleResolver.Create(settings.RootNamespace, Version.Parse(settings.Version));
        SetAttributes(module, moduleResolver, settings);

        var compilation = new DistIL.Compilation(module, new ConsoleLogger(), new CompilationSettings());
        var optimizer = new Optimizer();
        optimizer.CreatePassManager(compilation);

        return new Driver
        {
            Compilation = compilation,
            Settings = settings,
            Optimizer = optimizer,
            KnownAttributes = new KnownAttributes(moduleResolver),
            KnownTypes = new KnownTypes(moduleResolver)
        };
    }

    private static void SetAttributes(ModuleDef module, ModuleResolver moduleResolver, DriverSettings settings)
    {
        var targetFramework = new CustomAttrib(moduleResolver.Import(typeof(TargetFrameworkAttribute))
            .FindMethod(".ctor", new MethodSig(default, [PrimType.String])), [".NETCoreApp,Version=v9.0"]);

        module.GetCustomAttribs(true).Add(targetFramework);
    }

    public string GetNamespaceOf(AstNode node)
    {
        return node.Parent.Children.OfType<ModuleDeclaration>().FirstOrDefault()?.Canonicalize() ??
               Settings.RootNamespace;
    }

    public async Task Compile()
    {
        var hasError = () => PrintErrorsStage.Errors.Count >= 0;

        var pipeline = Pipeline.Build<Driver, Driver>(
            cfg =>
            {
                cfg.Add<ParsingStage>();
                cfg.Add<LoweringStage>();
                cfg.Add<ValidationStage>();
                cfg.Add<CollectTypesStage>();
                cfg.Add<CompileFunctionsStage>();
                cfg.Add<ImplementInterfacesStage>();
                cfg.Add<ApplyRulesStage>();
                cfg.Add<PrintErrorsStage>();

                cfg.When(_ => Settings.ShouldOptimize, _ =>
                {
                   _.Add<OptimizeStage>();
                });

                cfg.Add<SaveModuleStage>();
            }
        );

        await pipeline.Invoke(this);
    }

    public TypeDef GetFunctionType(string ns)
    {
        if (_functionTypes.TryGetValue(ns, out var value)) return value;

        var type = Compilation.Module.CreateType(ns, "Functions",
            TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);

        _functionTypes[ns] = type;

        return type;
    }
}