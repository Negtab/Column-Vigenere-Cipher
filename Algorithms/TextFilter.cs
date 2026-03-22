using System.Text;

namespace Crypto.Algorithms;

public static class TextFilter
{
    public static string FilterRussian(string input)
    {
        input = input.ToLower();

        StringBuilder result = new StringBuilder();

        foreach (char c in input)
        {
            if ((c >= 'а' && c <= 'я') || c == 'ё')
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    public static string FilterEnglish(string input)
    {
        input = input.ToLower();

        StringBuilder result = new StringBuilder();

        foreach (char c in input)
        {
            if (c >= 'a' && c <= 'z')
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }
}