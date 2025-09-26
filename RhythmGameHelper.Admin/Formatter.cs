using RhythmGameHelper.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Admin
{
    public class Formatter
    {

        public void FormatSplitter() => Repeat('-', 100);

        public void Repeat(char text, int times)
        {
            for (int i = 0; i < times; i++) Console.Write(text);
            Console.WriteLine("\n");
        }

        public void FormatException(string description)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(description);

            Console.ForegroundColor = color;
            Console.WriteLine();
        }

        public void FormatHeader()
        {
            Console.Clear();
            Console.WriteLine("RhythmGameHelper::PostgreSQL管理程序");
        }

        public void FormatTables(List<string> name)
        {
            FormatSplitter();
            Console.WriteLine("可查编表项：");
            Console.WriteLine();
            for (int i = 0; i < name.Count; i++)
            {
                Console.WriteLine($"   {i + 1}. {name[i]}");
            }
            Console.WriteLine();
        }

        public void FormatChoices(List<Choice> choices)
        {
            FormatSplitter();
            Console.WriteLine("输入项：");
            Console.WriteLine();
            for (int i = 0; i < choices.Count; i++)
            {
                Console.WriteLine($"   * {choices[i].Input} : {choices[i].Explanation}");
            }
            Console.WriteLine();
        }

        public void FormatTable(List<string> colNames, List<Dictionary<string, object>>? records)
        {
            (var lens,var strs, var disLens) = ConstructStrings(colNames, records ?? new());

            var bordersb = FormatTableBorder(lens);

            Console.WriteLine(bordersb);
            FormatTableColumnName(lens, colNames, -1);
            Console.WriteLine(bordersb);

            for (int i = 0; i < strs.Count; i++) FormatTableRecord(lens, strs[i], disLens[i]);

            Console.WriteLine(bordersb);
        }

        public void FormatTableMaxId(string? tableName, int maxId)
        {
            FormatSplitter();
            var defaultColor = Console.ForegroundColor;

            Console.Write("当前表： ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{tableName}");

            Console.ForegroundColor = defaultColor;
            Console.Write($"\t\t最大Id： ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{maxId}\n");

            Console.ForegroundColor= defaultColor;
            Console.WriteLine();
        }

        public void FormatTableHeader(string? tableName, int totalRecord,int currentPage, int totalPage)
        {
            FormatSplitter();
            var defaultColor = Console.ForegroundColor;

            Console.Write("当前表： ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{tableName}");

            Console.ForegroundColor= defaultColor;
            Console.Write($"\t\t元组数： ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{totalRecord}");

            Console.ForegroundColor = defaultColor;
            Console.Write("\t\t( ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{currentPage}");

            Console.ForegroundColor = defaultColor;
            Console.Write(" / ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{totalPage}");

            Console.ForegroundColor = defaultColor;
            Console.Write(" )\n");
            Console.WriteLine();
        }

        private StringBuilder FormatTableBorder(List<int> lens)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" +");
            foreach (int i in lens)
            {
                sb.Append('-', i);
                sb.Append('+');
            }
            return sb;
        }

        public void FormatTableColumnName(List<int> lens, List<string> names, int choice)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" |");
            for(int i = 0; i< Math.Min(lens.Count,names.Count); i++)
            {
                if(i == choice)
                {
                    var str = $"=> {names[i]}";
                    sb.Append(str.PadRight(lens[i] + 3 - str.DisplayLength() + str.Length));
                    sb.Append('|');
                }
                else
                {
                    sb.Append(names[i].PadRight(lens[i] - names[i].DisplayLength() + names[i].Length));
                    sb.Append('|');
                }
            }
            Console.WriteLine(sb.ToString());
        }

        private void FormatTableRecord(List<int> lens, List<string> values, List<int> displayLens)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" |");
            for (int i = 0; i < Math.Min(lens.Count, values.Count); i++)
            {
                sb.Append(values[i].PadRight(lens[i] - displayLens[i] + values[i].Length));
                sb.Append('|');
            }
            Console.WriteLine(sb.ToString());
        }

        private (List<int>,List<List<string>>,List<List<int>>) ConstructStrings(List<string> headers, List<Dictionary<string,object>> records)
        {
            var lens = new List<int>();
            var values = new List<List<string>>();
            var displayLengths = new List<List<int>>();

            for(int i = 0;i < headers.Count;++i) lens.Add(headers[i].Length);

            foreach (var dict in records)
            {
                var value = new List<string>();
                var displayLength = new List<int>();

                for (int i = 0; i < headers.Count; ++i)
                {
                    // 将每一个字段object转化为string
                    if (dict.TryGetValue(headers[i], out var v))
                    {
                        string str = (v is object[] vs) ? ConvertArray(vs) : $"{v}";

                        if (str == string.Empty)
                        {
                            value.Add("Null");
                            displayLength.Add(4);
                        }
                        else
                        {
                            value.Add(str);
                            displayLength.Add(str.DisplayLength());
                        }
                    }
                    else
                    {
                        value.Add("Null");
                        displayLength.Add(4);
                    }

                    lens[i] = Math.Max(lens[i], displayLength[i]);
                }

                values.Add(value);
                displayLengths.Add(displayLength);
            }

            return (lens, values, displayLengths);
        }

        private static string ConvertArray(object[] array)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            foreach (object item in array)
            {
                sb.Append($"{item},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append('}');
            return sb.ToString();
        }

        
    }
}
