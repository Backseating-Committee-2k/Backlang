using Socordia.CodeAnalysis.Parsing;
using SocordiaC;

namespace Socordia.Build.Tasks;

public class SocordiaBuildTask : LanguageSdk.Templates.Core.BuildTask<DriverSettings>
{
    protected override bool Execute(DriverSettings settings)
    {
        var driver = Driver.Create(settings);

        foreach (var reference in ReferencePaths)
            try
            {
                // Driver.moduleResolver.Load(reference.ItemSpec);
            }
            catch
            {
            }

        foreach(var er in EmbeddedResources)
        {
            var name = Path.GetFileName(er.ItemSpec);
            driver.Compilation.Module.CreateEmbeddedResource(name, File.ReadAllBytes(er.ItemSpec));
        }

        driver.Compile();

        foreach (var message in driver.Messages)
        {
            switch (message.Severity)
            {
                case MessageSeverity.Error:
                    Log.LogError(null, null, null,
                        file: message.Document.FileName, message.Range.Start.Line, message.Range.Start.Column,
                        message.Range.End.Line, message.Range.End.Column, message.Text);
                    break;
                case MessageSeverity.Warning:
                    Log.LogWarning(null, null, null,
                        file: message.Document.FileName, message.Range.Start.Line, message.Range.Start.Column,
                        message.Range.End.Line, message.Range.End.Column, message.Text);
                    break;
            }
        }

        return true;
    }
}