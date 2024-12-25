using System.Reflection;
using DistIL;
using DistIL.AsmIO;
using Flo;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.Parsing;
using SocordiaC.Compilation;
using SocordiaC.Stages;

namespace SocordiaC;

public class Driver
{
    public DriverSettings Settings { get; private init; } = new();
    public required DistIL.Compilation Compilation { get; set; }

    public required KnownAttributes KnownAttributes { get; set; }

    private Dictionary<string, TypeDef> _functionTypes { get; set; } = new();

    public Optimizer Optimizer { get; set; }
    public List<Message> Messages { get; set; } = [];
    public List<CompilationUnit> Trees { get; set; } = [];

    public static Driver Create(DriverSettings settings)
    {
        var moduleResolver = new ModuleResolver();
        moduleResolver.AddTrustedSearchPaths();

        var module = moduleResolver.Create(settings.RootNamespace, Version.Parse(settings.Version));

        var compilation = new DistIL.Compilation(module, new ConsoleLogger(), new CompilationSettings());
        var optimizer = new Optimizer();
        optimizer.CreatePassManager(compilation); ;

        return new Driver
        {
            Compilation = compilation,
            Settings = settings,
            Optimizer = optimizer,
            KnownAttributes = new KnownAttributes(moduleResolver)
        };
    }

    public string GetNamespaceOf(AstNode node)
    {
        return node.Parent.Children.OfType<ModuleDeclaration>().FirstOrDefault()?.Canonicalize() ?? Settings.RootNamespace;
    }

    public void Compile()
    {
        var hasError = (List<Message> messages) => messages.Any(_ => _.Severity == MessageSeverity.Error);

        var pipeline = Pipeline.Build<Driver, Driver>(
            cfg => {
                cfg.Add<ParsingStage>();
                cfg.Add<SemanticCheckStage>();
                cfg.Add<ConvertToIrStage>();

                cfg.Add<SaveModuleStage>();

                /*cfg.When(_ => !hasError(_.Messages) && _.Options.OutputTree, _ => {
                    _.Add<EmitTreeStage>();
                });

                cfg.When(_ => !hasError(_.Messages), _ => {
                    _.Add<InitStage>();
                });

                cfg.When(_ => !hasError(_.Messages), _ => {
                    _.Add<IntermediateStage>();
                });

                cfg.When(_ => !hasError(_.Messages), _ => {
                    _.Add<TypeInheritanceStage>();
                });

                cfg.When(_ => !hasError(_.Messages), _ => {
                    _.Add<ExpandImplementationStage>();
                });

                cfg.When(_ => !hasError(_.Messages), _ => {
                    _.Add<ImplementationStage>();
                });

                cfg.When(_ => !hasError(_.Messages), _ => {
                    _.Add<InitEmbeddedResourcesStage>();
                });

                cfg.When(_ => !hasError(_.Messages), _ => {
                    _.Add<CompileTargetStage>();
                });

                cfg.When(_ => _.Messages.Any(), _ => {
                    _.Add<ReportErrorStage>();
                });
                */
            }
        );

        pipeline.Invoke(this);
    }

    public TypeDef GetFunctionType(string ns)
    {
        if (_functionTypes.TryGetValue(ns, out TypeDef? value))
        {
            return value;
        }

        var type = Compilation.Module.CreateType(ns, "Functions",
            TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);

        _functionTypes[ns] = type;

        return type;
    }
}