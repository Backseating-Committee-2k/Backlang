using CommandLine;

namespace SocordiaC;

public static class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<DriverSettings>(args)
            .WithParsed(async void (options) =>
            {
                try
                {
                    var driver = Driver.Create(options);

                    await driver.Compile();
                }
                catch (Exception e)
                {
                    throw; // TODO handle exception
                }
            })
            .WithNotParsed(errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ToString());
                }
            });
    }
}