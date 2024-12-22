using CommandLine;

namespace SocordiaC;

public class DriverSettings : LanguageSdk.Templates.Core.DriverSettings
{
    //[Option('m', Required = false)]
    public IEnumerable<string>? MacroReferences { get; set; } = new List<string>();
}