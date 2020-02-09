using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort_ClassRealization
{
    static class MergeSort<T>
    {
        private static bool isIgnoring = false;
        // Для несортированных данных
        public static List<T> Sort(List<T> list, bool isString = true, bool isAscending = true)
        {
            if(list.Count == 1)
            {
                return list;
            }

            var mid = list.Count / 2;
            var left = list.Take(mid).ToList();
            var right = list.Skip(mid).ToList();

            return Merge(Sort(left, isString, isAscending), Sort(right, isString, isAscending));
        }
        
        //Для сортированных данных
        public static List<T> Merge(List<T> left, List<T> right, bool isString = true, bool isAscending = true)
        {
            var count = left.Count + right.Count;

            var leftPointer = 0;
            var rightPointer = 0;

            var resultList = new List<T>();

            for(int i = 0; i < count; i++)
            {
                if(leftPointer < left.Count && rightPointer < right.Count)
                {
                    if(Comparer(left[leftPointer], right[rightPointer], isString, isAscending))
                    {
                        resultList.Add(left[leftPointer]);
                        leftPointer++;
                    }
                    else
                    {
                        resultList.Add(right[rightPointer]);
                        rightPointer++;
                    }
                }
                else
                {
                    if(rightPointer < right.Count)
                    {
                        resultList.Add(right[rightPointer]);
                        rightPointer++;
                    }
                    else
                    {
                        resultList.Add(left[leftPointer]);
                        leftPointer++;
                    }
                }
            }

            return resultList;
        }

        private static bool Comparer(T first, T second, bool isString = true, bool isAscending = true)
        {
            if (isString)
            {
                if (string.Compare(first.ToString(), second.ToString()) == -1)
                {
                    return isAscending;
                }
                else
                {
                    return !isAscending;
                }
            }
            else
            {
                try
                {
                    if (int.Parse(first.ToString()) < int.Parse(second.ToString()))
                    {
                        return isAscending;
                    }
                    else
                    {
                        return !isAscending;
                    }
                }
                catch
                {
                    if(!isIgnoring)   Console.WriteLine("В одном или нескольких входных файлах данные имеют неверный тип. Проверьте данные во входных файлах. \n" +
                        "Нажмите кнопку Y, чтобы продолжить игнорируя ошибку");

                    if (Console.ReadKey().Key == ConsoleKey.Y) isIgnoring = true;
                }
            }

            return false;
        }
    }
        
    
}
