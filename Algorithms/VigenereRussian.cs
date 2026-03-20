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

    private static string Filter(string input)
    {
        StringBuilder sb = new StringBuilder();
        foreach(char c in input)
        {
            char lower = char.ToLower(c);
            if(AlphabetIndex.ContainsKey(lower))
                sb.Append(lower);
        }
        return sb.ToString();
    }

    public static string Encrypt(string text, string key)
    {
        text = Filter(text);
        key = Filter(key);

        StringBuilder result = new StringBuilder();
        
        string fullKey = key;
        
        int count = 1;
        for (int i = key.Length; i < text.Length; i++)
        {
            count = (i / key.Length > count) ?  count + 1 : count;
            fullKey += (char)((int)key[i % key.Length] + count);
        }
            
            

        for (int i = 0; i < text.Length; i++)
        {
            int t = AlphabetIndex[text[i]];
            int k = AlphabetIndex[fullKey[i]];

            int pos = (t + k) % Len;
            result.Append(Alphabet[pos]);
        }

        return result.ToString();
    }

    public static string Decrypt(string text, string key)
    {
        text = Filter(text);
        key = Filter(key);

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            int k;

            if (i < key.Length)
                k = AlphabetIndex[key[i]];
            else
                k = AlphabetIndex[result[i - key.Length]];

            int t = AlphabetIndex[text[i]];
            int pos = (t - k + Len) % Len;

            char decrypted = Alphabet[pos];
            result.Append(decrypted);
        }

        return result.ToString();
    }
}