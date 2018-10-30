using System;
using System.Collections.Generic;
using System.Linq;

/*
 * 2. Дана коллекция List<T>. Требуется подсчитать, сколько раз каждый элемент встречается в данной коллекции:
 * a. для целых чисел;
 * b. * для обобщенной коллекции;
 * c. ** используя Linq.
*/
public static class MyExtensions
{
    /// <summary>
    /// Метод для вывода информации по всем значениям IDictionary
    /// </summary>
    /// <param name="dict"></param>
    public static void UniqueValuesToString(this IDictionary<int, int> dict)
    {
        foreach (KeyValuePair<int, int> pair in dict)
        {
            Console.WriteLine($"В данной коллекции, значение: {pair.Key} повторяется {pair.Value} {(((pair.Value % 2 == 0) || (pair.Value == 3) || (pair.Value != 6) && (pair.Value > 6)) ? "раза" : "раз")}");
        }
    }
}

namespace MyGame
{
    class SecondTask
    {
        /// <summary>
        /// Метод находит повторяющиеся значения в коллекции List и возвращает коллекцию IDictionary T, int, где T - уникальное значение, int - количество вовторений уникального значения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IDictionary<T, int> GetUniques<T>(ICollection<T> list)
        {
            Dictionary<T, int> found = new Dictionary<T, int>();
            foreach (T val in list)
            {
                if (!found.ContainsKey(val))
                {
                    found.Add(val, 1);
                }
                else
                {
                    found[val]++;
                }
            }
            return found;
        }

        public static void Second()
        {
            Random rnd = new Random();
            var integerList = new List<int>();
            // коллекция уникальных значений, и количества их повторений
            var countUnique = new Dictionary<int, int>();
            // количество значений в коллекции integerList
            int c = rnd.Next(5, 15);

            for (int i = 0; i < c; i++)
                integerList.Add(rnd.Next(1, 10));
            
            foreach (int obj in integerList)
                if (countUnique.ContainsKey(obj))
                    countUnique[obj]++;
                else
                    countUnique.Add(obj, 1);

            // 2.a
            Console.WriteLine("Дана коллекция List<T>. Требуется подсчитать, сколько раз каждый элемент встречается в данной коллекции для целых чисел:");
            countUnique.UniqueValuesToString();

            // 2.b
            Console.WriteLine("\nДля обобщенной коллекции:");
            GetUniques(integerList).UniqueValuesToString();

            // 2.c
            Console.WriteLine("\nИспользуя Linq:");
            var unique = new List<int>();
            foreach (int obj in integerList)
            {
                if (!unique.Contains(obj))
                {
                    unique.Add(obj);
                    int t = integerList.Where(x => x == obj).Count();
                    Console.WriteLine($"В данной коллекции, значение: {obj} повторяется {t} { (((t % 2 == 0) || (t == 3) || (t != 6) && (t > 6)) ? " раза" : " раз")}");
                }
            }

            Console.WriteLine("\nДля продолжения нажмите любую кнопку");
            System.Console.ReadKey();
            Console.Clear();
        }
    }
}