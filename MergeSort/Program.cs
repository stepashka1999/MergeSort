using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    static bool isIgnoring = false;
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
                if(!isIgnoring)
                Console.WriteLine("Неподходящий тип данных в одном из входящих файлов. Пожалуйста перепроверьте входные данные.\n" +
                    " Нажмите клавишу Y, чтобы проигнорировать ошибку и продолжить.");
                
                if (Console.ReadKey().Key == ConsoleKey.Y) isIgnoring = true;
            }
        }

        if (!isAscending) res = !res;

        return res;
    }
    
    //метод для слияния массивов
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

        for (var i = left; i <= middleIndex; i++)
        {
            tempArray[index] = array[i];
            index++;
        }

        for (var i = right; i <= highIndex; i++)
        {
            tempArray[index] = array[i];
            index++;
        }

        for (var i = 0; i < tempArray.Length; i++)
        {
            array[lowIndex + i] = tempArray[i];
        }
    }

    //сортировка слиянием
    static string[] MergeSort(string[] array, int lowIndex, int highIndex, bool isAscending, bool isString)
    {
        if (lowIndex < highIndex)
        {
            var middleIndex = (lowIndex + highIndex) / 2;
            MergeSort(array, lowIndex, middleIndex, isAscending, isString);
            MergeSort(array, middleIndex + 1, highIndex, isAscending, isString);
            Merge(array, lowIndex, middleIndex, highIndex, isAscending, isString);
        }

        return array;
    }

    static string[] MergeSort(string[] array, bool isAscending, bool isString)
    {
        return MergeSort(array, 0, array.Length - 1, isAscending, isString);
    }
    
    public static void Main(string[] args)
    {
        //входные параметры
        bool isAscending = true; // Дефолтное значение - по возрастанию
        bool isString = true;  // Дефолтное значение входных данных - строка

        string WritePath = String.Empty;
        string Path =  @"D:\\VisualStudioProjects\\MergeSort\\MergeSort\\InputFiles\\";


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
                if (el[i].ToCharArray().Last() == '\r') el[i] = el[i].Remove(el[i].Length-1);
            }
        }

        // Объединяем считанные данные в один лист
        for (int i = 1; i < TempArrayToSort.Count; i++)
        {
            list = TempArrayToSort[0].Concat(TempArrayToSort[i]);
        }
                
        var SortedArray = MergeSort(list.ToArray(), isAscending, isString);
        
        foreach (var s in SortedArray)
        {
            Console.WriteLine(s);
        }

        //WriteToFile
        WriteToFile(Path + WritePath, SortedArray);
        
        Console.ReadLine();
    }

    //Read From File
    static string[] ReadFormFile(string path)
    {
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
}