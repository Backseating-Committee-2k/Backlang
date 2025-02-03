namespace Socordia.CodeAnalysis.Parsing;

public static class ParsingUtils
{
    public static string ToPascalCase(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        return char.ToUpper(name[0]) + name[1..];
    }
}