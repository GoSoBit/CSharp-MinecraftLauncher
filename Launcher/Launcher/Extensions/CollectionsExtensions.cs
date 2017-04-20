using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Launcher.Extensions
{
    public static class CollectionsExtensions
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }

        public static void AddOrUpdate<T>(this List<T> enumerable, T newItem, Predicate<T> predicate)
        {
            int itemIndex = enumerable.FindIndex(predicate);
            if (itemIndex >= 0)
            {
                enumerable[itemIndex] = newItem;
            }
            else
            {
                enumerable.Insert(0, newItem);
            }
        }
    }
}