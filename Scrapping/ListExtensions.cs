using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinEmailExtract
{
    public static class ListExtensions
    {
       public static void RemoveDuplicates<T>(this List<T> list, Comparison<T> comparison)
        {
              for (int i = 0; i < list.Count; i++)
                {
                          for (int j = list.Count-1; j > i; j--)
                            {
                                        if (comparison(list[i], list[j]) == 0)
                                            list.RemoveAt(j);
                            }
                }
        }
    }    
}
