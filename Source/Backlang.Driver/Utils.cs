using Backlang.Core.CompilerService;
using Furesoft.Core.CodeDom.Compiler.TypeSystem;
using System.Runtime.CompilerServices;

namespace Backlang.Driver;

public static class Utils
{
    public static string GenerateIdentifier()
    {
        var sb = new StringBuilder();
        const string ALPHABET = "abcdefhijklmnopqrstABCDEFGHIJKLMNOPQRSTUVWXYZ&%$";
        var random = new Random();

        for (var i = 0; i < random.Next(5, 9); i++)
        {
            sb.Append(ALPHABET[random.Next(ALPHABET.Length)]);
        }

        return sb.ToString();
    }

    public static void AddCompilerGeneratedAttribute(TypeResolver binder, DescribedType type)
    {
        var attributeType = ResolveType(binder, typeof(CompilerGeneratedAttribute));

        type.AddAttribute(new DescribedAttribute(attributeType));
    }
}