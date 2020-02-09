using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSortForSortingData
{
    class Program
    {
        static bool isIgnoring = false;
        public static void Main(string[] args)
        {
            //входные параметры
            bool isAscending = true; // Дефолтное значение - по возрастанию
            bool isString = true;  // Дефолтное значение входных данных - строка

            string WritePath = String.Empty;
            string Path = @"D:\\VisualStudioProjects\\MergeSort\\MergeSort\\InputFiles\\";


            Console.WriteLine("Сортировка слиянием");

            List<string[]> TempArrayToSort = new List<string[]>(); // Лист для записи считываемых файлов
            IEnumerable<string> list = new List<string>(); // Лист для объединения данных из считываемых файлов

            //Считываем аргументы
            foreach (var a in args)
            {
                if (a == "-d") isAscending = false;
                if (a == "-i") isString = false;

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

            var MergedList = Merge(TempArrayToSort, isAscending, isString);
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

        static void Merge(string[] array, int lowIndex, int middleIndex, int highIndex, bool isAscending, bool isString)
        {
            var left = lowIndex;
            var right = middleIndex + 1;
            var tempArray = new string[highIndex - lowIndex + 1];
            var index = 0;


            while ((left <= middleIndex) && (right <= highIndex))
            {
                if (Max(array[left], array[right], isAscending, isString))
                {
                    tempArray[index] = array[left];
                    left++;
                }
                else
                {
                    tempArray[index] = array[right];
                    right++;
                }

                index++;
            }

            for (var i = 0; i < tempArray.Length; i++)
            {
                array[lowIndex + i] = tempArray[i];
            }
        }

        static string[] Merge(List<string[]> listOfArray, bool isAscending, bool isString)
        {
            while(listOfArray.Count != 1)
            {
                var tempStr = new string[listOfArray[0].Length + listOfArray[1].Length];
                var index = 0;
                for(int i = 0; i < 2; i++)
                    for(int j =0; j < listOfArray[i].Length; j++)
                    {
                        tempStr[index] = listOfArray[i][j];
                        index++;
                    }

                var middleIndex = listOfArray[0].Length + 1;

                Merge(tempStr, 0, middleIndex, tempStr.Length - 1, isAscending, isString);

                listOfArray.RemoveRange(0, 2);
                listOfArray.Add(tempStr);
            }

            return listOfArray[0].ToArray();
        }

        static bool Max(string v1, string v2, bool isAscending, bool isString)
        {

            bool res = true;
            if (isString) res = v1.Length < v2.Length;
            else
            {
                try
                {
                    res = int.Parse(v1) < int.Parse(v2);
                }
                catch
                {
                    if (!isIgnoring)
                        Console.WriteLine("Неподходящий тип данных в одном из входящих файлов. Пожалуйста перепроверьте входные данные.\n" +
                            " Нажмите клавишу Y, чтобы проигнорировать ошибку и продолжить.");

                    if (Console.ReadKey().Key == ConsoleKey.Y) isIgnoring = true;
                }
            }

            if (!isAscending) res = !res;

            return res;
        }

    }
}
