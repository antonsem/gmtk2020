using System;
using System.Collections.Generic;

namespace Coderman
{
    public static class Extensions
    {
        private static Random rng = new Random();  

        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

        public static int GetCount<T>(this Queue<T> list, T obj)
        {
            int count = 0;

            foreach (T item in list)
                if (item.Equals(obj)) count++;

            return count;
        }
    }
}
