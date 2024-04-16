namespace Task2;

public class Program
{
    public static void Main()
    {
        Solve();
    }
    public static void Solve()
    {
        // helpers to test solution
        var input1 = ReadInts(Console.ReadLine());
        var input2 = ReadInts(Console.ReadLine());
        var m = input1[0];
        var n = input1[1];
        int k = 0;
        int[,] matrix = new int[m, n];
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = input2[k++];
            }
        }
        var res = SpiralMatrix(matrix);
        foreach (var r in res) Console.Write($"{r} ");
    }
    // solution method for task 2
    public static int[] SpiralMatrix(int[,] matrix)
    {
        int m = matrix.GetLength(0), n = matrix.GetLength(1);
        int x1 = 0, y1 = 0, x2 = m - 1, y2 = n - 1;
        List<int> ans = new();

        while (x1 <= x2 && y1 <= y2)
        {
            for (int i = x1; i <= x2; ++i)
            {
                ans.Add(matrix[i, y1]);
            }
            for (int j = y1 + 1; j <= y2; ++j)
            {
                ans.Add(matrix[x2, j]);
            }
            if (x1 < x2 && y1 < y2)
            {
                for (int i = x2 - 1; i >= x1; --i)
                {
                    ans.Add(matrix[i, y2]);
                }
                for (int j = y2 - 1; j > y1; --j)
                {
                    ans.Add(matrix[x1, j]);
                }
            }
            x1++;
            y1++;
            x2--;
            y2--;
        }

        return ans.ToArray();
    }
    private static int[] ReadInts(string input)
    {
        return input.Split(" ")
                    .Select(int.Parse)
                    .ToArray();
    }

}
