using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MergeSort_ClassRealization
{
    class Program
    {
        public static void Main(string[] args)
        {
            //входные параметры
            bool isAscending = true; // Дефолтное значение - по возрастанию
            bool isString = true;  // Дефолтное значение входных данных - строка
            bool isSorted = true; // Дефолтное значение входных данных - сортированные

            string WritePath = String.Empty;
            string Path = @"D:\\VisualStudioProjects\\MergeSort\\MergeSort\\InputFiles\\";

            List<string[]> TempArrayToSort = new List<string[]>(); // Лист для записи считываемых файлов
            IEnumerable<string> list = new List<string>(); // Лист для объединения данных из считываемых файлов

            //Считываем аргументы
            foreach (var a in args)
            {
                if (a == "-d") isAscending = false;
                if (a == "-i") isString = false;
                if (a == "-ns") isSorted = false;

                if (a.EndsWith(".txt"))
                {
                    if (String.IsNullOrEmpty(WritePath)) WritePath = a;
                    else
                    {
                        //ReadFromFile
                        TempArrayToSort.Add(ReadFormFile(Path + a));
                    }
                }
            }

            //Чистим строки от символа перехода но новую строку
            foreach (var el in TempArrayToSort)
            {
                for (int i = 0; i < el.Length; i++)
                {
                    if (el[i].ToCharArray().Last() == '\r') el[i] = el[i].Remove(el[i].Length - 1);
                }
            }

            string[] MergedList;
            
            if(isSorted) MergedList = Merge(TempArrayToSort, isAscending, isString);
            else
            {
                MergedList = NonSortedMerge(TempArrayToSort, isString, isAscending);
            }

            WriteToFile(Path + WritePath, MergedList);
        }
        //Read From File
        static string[] ReadFormFile(string path)
        {
            Console.WriteLine(path);
            StreamReader streamReader;
            streamReader = new StreamReader(path);
            var temRes = streamReader.ReadToEnd().Split('\n');
            streamReader.Close();
            return temRes;
        }

        //Write To File
        static void WriteToFile(string path, string[] array)
        {
            StreamWriter streamWriter;
            streamWriter = new StreamWriter(path, true, System.Text.Encoding.Default);

            foreach (var s in array)
            {
                streamWriter.WriteLine(s);
            }
            streamWriter.Close();

        }

        static string[] NonSortedMerge(List<string[]> list, bool isString = true, bool isAscending = true)
        {
            List<string> result = new List<string>();

            foreach(var el in list)
                foreach(var subEl in el)
                {
                    result.Add(subEl);
                }

            return MergeSort<string>.Sort(result, isString, isAscending).ToArray();
        }
        static string[] Merge(List<string[]> list, bool isString = true, bool isAscending = true)
        {
            if (list.Count == 1) return list[0].ToArray();


            while(list.Count != 1)
            {
                var left = list[0].ToList();
                var right = list[1].ToList();

                var result = MergeSort<string>.Merge(left, right, isString, isAscending);

                list.RemoveAt(1);

                list[0] = result.ToArray();

            }

            return list[0].ToArray();
        }
    }
}
