using Socordia.CodeAnalysis.AST;
using System.Collections.Immutable;

namespace Socordia.CodeAnalysis.Parsing.ParsePoints;

public sealed class ModifierParser
{
    private static readonly ImmutableDictionary<TokenType, Modifier> PossibleModifiers =
        new Dictionary<TokenType, Modifier>
        {
            { TokenType.Static, Modifier.Static },
            { TokenType.Public, Modifier.Public },
            { TokenType.Protected, Modifier.Protected },
            { TokenType.Private, Modifier.Private },
            { TokenType.Internal, Modifier.Internal },
            { TokenType.Operator, Modifier.Operator },
            { TokenType.Abstract, Modifier.Abstract },
            { TokenType.Override, Modifier.Override },
            { TokenType.Extern, Modifier.Extern }
        }.ToImmutableDictionary();

    public static bool TryParse(Parser parser, out List<Modifier> node)
    {
        var modifiers = new List<Modifier>();

        while (PossibleModifiers.ContainsKey(parser.Iterator.Current.Type))
        {
            var modifier = ParseSingle(parser.Iterator);
            if (modifiers.Contains(modifier))
            {
                continue;
            }

            modifiers.Add(modifier);
        }

        node = modifiers;

        return modifiers.Count > 0;
    }

    private static Modifier ParseSingle(TokenIterator iterator)
    {
        var currentToken = iterator.Current;
        var mod = PossibleModifiers[currentToken.Type];
        iterator.NextToken();

        return mod;
    }
}