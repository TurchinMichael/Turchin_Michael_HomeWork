using System;
using System.Collections.Generic;
using System.Linq;

/*
 * 3. * Дан фрагмент программы:
 * а. Свернуть обращение к OrderBy с использованием лямбда-выражения =>.
 * b. * Развернуть обращение к OrderBy с использованием делегата.
*/
namespace MyGame
{
    delegate int pairDelegate(KeyValuePair<string, int> dict);
    class ThirdTask
    {
        public static void Third()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>() { { "four", 4 }, { "two", 2 }, { "one", 1 }, { "three", 3 } };

            #region 3.а. Свернуть обращение к OrderBy с использованием лямбда-выражения =>.
            Console.WriteLine("Свернуть обращение к OrderBy с использованием лямбда-выражения =>");
            foreach (var pair in dict.OrderBy(i => i.Value))
                Console.WriteLine($"{pair.Key,5} - {pair.Value,2}");
            #endregion

            #region 3.b. * Развернуть обращение к OrderBy с использованием делегата. Получилось с использованием полноценного метода
            Console.WriteLine("\nРазвернуть обращение к OrderBy с использованием делегата");
            var m = dict.OrderBy(pairMethod);
            foreach (var pair in m)
                Console.WriteLine($"{pair.Key,5} - {pair.Value,2}");
            #endregion

            #region через делегат не получилось :C
            //pairDelegate _pairDelegate = new pairDelegate(pairMethod);
            //var d = dict.OrderBy(_pairDelegate);
            #endregion

            Console.WriteLine("\nДля продолжения нажмите любую кнопку");
            System.Console.ReadKey();
            Console.Clear();
        }

        static int pairMethod(KeyValuePair<string, int> dict)
        { return dict.Value; }
    }
}