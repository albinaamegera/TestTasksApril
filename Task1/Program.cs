using System.Text;

namespace Task1
{
    class Program
    {
        public static void Main()
        {
            Solve();
        }
        // simple input to test the solution
        private static void Solve()
        {
            var input = Console.ReadLine();

            var compressed = Compress(input);
            var decompressed = Decompress(compressed);

            Console.WriteLine(compressed);
            Console.WriteLine(decompressed);
        }

        #region solution methods for task 1

        public static string Compress(string str)
        {
            StringBuilder builder = new();

            var lastsym = str[0];
            var lastindex = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != lastsym)
                {
                    builder.Append(Pack(lastsym, i - lastindex));
                    lastsym = str[i];
                    lastindex = i;
                }
            }

            builder.Append(Pack(lastsym, str.Length - lastindex));

            return builder.ToString();

            string Pack(char c, int count)
            {
                if (count > 1)
                {
                    return $"{c}{count}";
                }
                return c.ToString();
            }
        }
        public static string Decompress(string str)
        {
            StringBuilder builder = new();
            var commonstr = ToCommon(str);

            for (int i = 1; i < commonstr.Length; i += 2)
            {
                builder.Append(Unpack(commonstr[i - 1], commonstr[i]));
            }

            return builder.ToString();

            string ToCommon(string str)
            {
                StringBuilder builder = new();

                if (!int.TryParse(str.Last().ToString(), out int m))
                {
                    str += "1";
                }

                var lastsym = str[0];
                var isLastDigit = false;

                for (int i = 1; i < str.Length; i++)
                {
                    var isDigit = int.TryParse(str[i].ToString(), out int _count);

                    if (!isLastDigit && isDigit)
                    {
                        builder.Append(lastsym);
                        builder.Append(_count);
                        isLastDigit = true;
                    } 
                    else if (isLastDigit && !isDigit)
                    {
                        isLastDigit = false;
                        lastsym = str[i];
                    } 
                    else
                    {
                        builder.Append(lastsym);
                        builder.Append(1);
                        lastsym = str[i];
                    }
                }
                return builder.ToString();
            }

            string Unpack(char c, char count)
            {
                var digit = int.Parse(count.ToString());
                if (digit > 1)
                {
                    string str = "";
                    for (int i = 0; i < digit; i++) str += c;
                    return str;
                }
                return c.ToString();
            }
        }

        #endregion

    }
}
