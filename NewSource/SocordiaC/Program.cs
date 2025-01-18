using CommandLine;

namespace SocordiaC;

public static class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<DriverSettings>(args)
            .WithParsed(async void (options) =>
            {
                var driver = Driver.Create(options);

                await driver.Compile();
            })
            .WithNotParsed(errors =>
            {
                foreach (var error in errors) Console.WriteLine(error.ToString());
            });
    }
}