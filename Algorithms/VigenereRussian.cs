using System;
using System.Collections.Generic;
using System.Linq;

namespace Crypto.Algorithms;
using System.Text;

public static class VigenereRussian
{
    private static readonly char[] Alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
    private static readonly int Len = Alphabet.Length;
    private static readonly Dictionary<char,int> AlphabetIndex = Alphabet.Select((c,i)=> new {c,i}).ToDictionary(x=>x.c, x=>x.i);
    
    private static char ShiftChar(char c, int shift)
    {
        int idx = AlphabetIndex[c];
        return Alphabet[(idx + shift) % Len];
    }

    public static string Encrypt(string text, string key)
    {
        StringBuilder result = new StringBuilder();
        StringBuilder fullKey = new StringBuilder(key);

        int count = 0;

        for (int i = key.Length; i < text.Length; i++)
        {
            if (i % key.Length == 0)
                count++;

            fullKey.Append(ShiftChar(key[i % key.Length], count));
        }

        for (int i = 0; i < text.Length; i++)
        {
            int t = AlphabetIndex[text[i]];
            int k = AlphabetIndex[fullKey[i]];

            result.Append(Alphabet[(t + k) % Len]);
        }

        return result.ToString();
    }

    public static string Decrypt(string text, string key)
    {
        StringBuilder result = new StringBuilder();
        StringBuilder fullKey = new StringBuilder(key);

        int count = 0;

        for (int i = key.Length; i < text.Length; i++)
        {
            if (i % key.Length == 0)
                count++;

            fullKey.Append(ShiftChar(key[i % key.Length], count));
        }

        for (int i = 0; i < text.Length; i++)
        {
            int t = AlphabetIndex[text[i]];
            int k = AlphabetIndex[fullKey[i]];

            result.Append(Alphabet[(t - k + Len) % Len]);
        }

        return result.ToString();
    }
}