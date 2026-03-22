namespace Crypto.Algorithms;

using System;
using System.Linq;
using System.Text;

public static class ColumnarProgressive
{
    private static int[] GetColumnOrder(string key)
    {
        return key
            .Select((c, i) => new { Char = c, Index = i })
            .OrderBy(x => x.Char)
            .ThenBy(x => x.Index)
            .Select(x => x.Index)
            .ToArray();
    }

    public static string Encrypt(string text, string key)
    {
        text = text.Replace(" ", "").ToLower();
        key = key.ToLower();

        int cols = key.Length;
        int rows = (int)Math.Ceiling((double)text.Length / cols);

        char[,] table = new char[rows, cols];

        int index = 0;

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (index < text.Length)
                    table[r, c] = text[index++];
                else
                    table[r, c] = '\0'; // пустая ячейка

        int[] order = GetColumnOrder(key);

        StringBuilder result = new StringBuilder();

        foreach (var col in order)
            for (int r = 0; r < rows; r++)
                if (table[r, col] != '\0')
                    result.Append(table[r, col]);

        return result.ToString();
    }

    public static string Decrypt(string text, string key)
    {
        text = text.ToLower();
        key = key.ToLower();

        int cols = key.Length;
        int rows = (int)Math.Ceiling((double)text.Length / cols);

        int fullCells = text.Length;
        int emptyCells = rows * cols - fullCells;

        int[] order = GetColumnOrder(key);

        int[] colLengths = new int[cols];

        for (int i = 0; i < cols; i++)
            colLengths[i] = rows;

        for (int i = cols - emptyCells; i < cols; i++)
            colLengths[i]--;

        char[,] table = new char[rows, cols];

        int index = 0;

        foreach (var col in order)
        {
            int len = colLengths[col];

            for (int r = 0; r < len; r++)
                table[r, col] = text[index++];
        }

        StringBuilder result = new StringBuilder();

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (table[r, c] != '\0')
                    result.Append(table[r, c]);

        return result.ToString();
    }
}