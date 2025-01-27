﻿// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    public static class IListExtensions
    {
        /// <summary>
        /// Get the item at the specified index, automatically limiting
        /// the index to the bounds of the list.  Entering an index less than
        /// zero will return the first item.  Entering an index greater than the
        /// size of the array will return the last item.
        /// Optionally, the indexing may be reversed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index">The index.  Will be bounded to the list domain.</param>
        /// <param name="reverse">Optional.  If true the indexing will be reversed.</param>
        /// <returns></returns>
        public static T GetBounded<T>(this IList<T> list, int index, bool reverse = false)
        {
            if (reverse) index = list.Count - 1 - index;
            if (index >= list.Count) index = list.Count - 1;
            if (index < 0) index = 0;
            return list[index];
        }

        /// <summary>
        /// Get the item at the specified index, or the default value
        /// of the relevant type if the index is outside the bounds of the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetOrDefault<T>(this IList<T> list, int index)
        {
            if (index >= list.Count) return default(T);
            if (index < 0) return default(T);
            return list[index];
        }

        /// <summary>
        /// Get the item at the specified index, automatically wrapping if it is outside the
        /// bounds of the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="wrap">Wrapping toggle - if false the wrapping will not occur and the list
        /// will be accessed normally.</param>
        /// <returns></returns>
        public static T GetWrapped<T>(this IList<T> list, int index, bool wrap = true)
        {
            if (wrap && list.Count > 0)
            {
                while (index >= list.Count) index -= list.Count;
                while (index < 0) index += list.Count;
            }
            return list[index];
        }

        /// <summary>
        /// Get the item the specified number of places from the end of this
        /// list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T FromEnd<T>(this IList<T> list, int index)
        {
            return list[list.Count - 1 - index];
        }


        /// <summary>
        /// Remove duplicate objects from this collection, leaving
        /// the first discovered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveDuplicates<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                T itemA = list[i];
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (itemA.Equals(list[j]))
                    {
                        list.RemoveAt(j);
                    }
                }
            }
        }

        /// <summary>
        /// Remove *all* duplicate objects from this collection, including
        /// the first instance discovered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveAllDuplicates<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                T itemA = list[i];
                bool remove = false;
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (itemA.Equals(list[j]))
                    {
                        list.RemoveAt(j);
                        remove = true;
                    }
                }
                if (remove)
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Return the sub-list of all items in this list between the two specified index parameters.
        /// If the end value is lower than the start value, the item selection will wrap.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="start">The start of the index range to extract</param>
        /// <param name="end">The end of the index range to extract</param>
        /// <returns></returns>
        public static IList<T> AllBetween<T>(this IList<T> list, double start, double end)
        {
            IList<T> result = new List<T>();
            if (end > start)
            {
                for (int i = (int)Math.Ceiling(start); i < Math.Min(list.Count, end); i++)
                {
                    result.Add(list[i]);
                }
            }
            else
            {
                for (int i = (int)Math.Ceiling(start); i < list.Count; i++)
                {
                    result.Add(list[i]);
                }
                for (int i = 0; i < end; i++)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// Get the index of the last item in the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int LastIndex<T>(this IList<T> list)
        {
            return list.Count - 1;
        }

        /// <summary>
        /// Remove the last item in this list, for
        /// safety checking first whether there are any items within
        /// the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveLast<T>(this IList<T> list)
        {
            int i = list.LastIndex();
            if (i >= 0) list.RemoveAt(i);
        }

        /// <summary>
        /// Remove the first item in this list,
        /// for safety checking first whether there are any items to remove.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveFirst<T>(this IList<T> list)
        {
            if (list.Count > 0) list.RemoveAt(0);
        }

        /// <summary>
        /// Get the sublist containing the item at the specified index and
        /// every subsequent item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static IList<T> SubListFrom<T>(this IList<T> list, int index)
        {
            var result = new List<T>(list.Count - index);
            for (int i = index; i < list.Count; i++)
            {
                result.Add(list[i]);
            }
            return result;
        }

        /// <summary>
        /// Find the maximum value of a property or method on the items in this list
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value of the property
        /// for each item in this list.</param>
        /// <returns></returns>
        public static TProperty MaxDelegateValue<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TProperty);
            TProperty result = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TProperty value = propertyDelegate.Invoke(list[i]);
                if (value.CompareTo(result) == 1) result = value; 
            }
            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the minimum value of a property or method
        /// defined by a delegate
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <returns></returns>
        public static TItem ItemWithMax<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TItem);
            TItem result = list[0];
            TProperty max = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(max) == 1)
                {
                    max = value;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the minimum value of a property or method on the items in this list
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <returns></returns>
        public static TProperty MinDelegateValue<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TProperty);
            TProperty result = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TProperty value = propertyDelegate.Invoke(list[i]);
                if (value.CompareTo(result) == -1) result = value;
            }
            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the minimum value of a property or method
        /// defined by a delegate
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <returns></returns>
        public static TItem ItemWithMin<TItem, TProperty>(this IList<TItem> list, Func<TItem, TProperty> propertyDelegate)
            where TProperty : IComparable<TProperty>
        {
            if (list.Count == 0) return default(TItem);
            TItem result = list[0];
            TProperty min = propertyDelegate.Invoke(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(min) == -1)
                {
                    min = value;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the index of the minimum value in this list
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int IndexOfMin<TItem>(this IList<TItem> list)
            where TItem : IComparable<TItem>
        {
            if (list.Count == 0) return -1;
            TItem min = list[0];
            int result = 0;
            for (int i = 0; i < list.Count; i++)
            {
                TItem value = list[i];
                if (value.CompareTo(min) == -1)
                {
                    min = value;
                    result = i;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the next highest value of a property or method
        /// defined by a delegate after the specified value
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <param name="after">The value for which the subsequent value in order is being sought</param>
        /// <returns></returns>
        public static TItem ItemWithNext<TItem, TProperty>(this IList<TItem> list, 
            Func<TItem, TProperty> propertyDelegate, TProperty after)
            where TProperty : IComparable<TProperty>
            where TItem : class
        {
            TItem result = null;
            TProperty min = default(TProperty);
            for (int i = 0; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(after) == 1 && 
                    (result == null ||value.CompareTo(min) == -1))
                {
                    min = value;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Find the item in this list which returns the highest value of a property or method
        /// defined by a delegate before the specified value
        /// </summary>
        /// <typeparam name="TItem">The type of item in the list</typeparam>
        /// <typeparam name="TProperty">The type of the property to be interrogated</typeparam>
        /// <param name="list"></param>
        /// <param name="propertyDelegate">Delegate function which returns the value
        /// for each list item.</param>
        /// <param name="before">The value for which the subsequent value in order is being sought</param>
        /// <returns></returns>
        public static TItem ItemWithPrevious<TItem, TProperty>(this IList<TItem> list,
            Func<TItem, TProperty> propertyDelegate, TProperty before)
            where TProperty : IComparable<TProperty>
            where TItem : class
        {
            TItem result = null;
            TProperty max = default(TProperty);
            for (int i = 1; i < list.Count; i++)
            {
                TItem item = list[i];
                TProperty value = propertyDelegate.Invoke(item);
                if (value.CompareTo(before) == -1 && 
                    (result == null || value.CompareTo(max) == 1))
                {
                    max = value;
                    result = item;
                }
            }
            return result;
        }

        /// <summary>
        /// Shift all items in this list by the specified number of places.
        /// An items moving off the start or end of the list will 'wrap' to the other
        /// end of the list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="offset">The number of places to shift the objects in this list.
        /// +ve numbers are to the left (i.e items will move towards the start of the list)
        /// and -ve numbers are to the right (i.e. items move towards the end).</param>
        /// <param name="wrap">If true, items shifted off the start or end of the list
        /// will be added to the other end of the list.  If false, they will be lost and the
        /// size of the list will decrease.</param>
        public static void Shift<TItem>(this IList<TItem> list, int offset)
        {
            // Store items that move off the start or end of the list
            TItem[] original = list.ToArray();

            // Shift the list items
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = original.GetWrapped(i + offset);
            }
        }

        /// <summary>
        /// Find the average of a set of double values obtainable via a delegate function from the items
        /// in this list.
        /// </summary>
        /// <typeparam name="TItem">The type of item in this list.</typeparam>
        /// <param name="list"></param>
        /// <param name="valueDelegate">Delegate function which returns the value to be averaged for each list item.</param>
        /// <returns></returns>
        public static double AverageDelegateValue<TItem>(this IList<TItem> list, Func<TItem, double> valueDelegate)
        {
            double result = 0;
            for (int i = 0; i < list.Count; i++) result += valueDelegate.Invoke(list[i]);
            if (list.Count > 0) result /= list.Count;
            return result;
        }

        /// <summary>
        /// Find the total of a set of double values obtainable via a delegate function from the
        /// items in this list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="valueDelegate">Delegate function which returns the value to the summed.</param>
        /// <returns></returns>
        public static double TotalDelegateValue<TItem>(this IList<TItem> list, Func<TItem, double> valueDelegate)
        {
            double result = 0;
            for (int i = 0; i < list.Count; i++) result += valueDelegate.Invoke(list[i]);
            return result;
        }

        /// <summary>
        /// Find the total of a set of double values obtainable via a delegate function from the
        /// items in this list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="valueDelegate">Delegate function which returns the value to the summed.</param>
        /// <returns></returns>
        public static int TotalDelegateValue<TItem>(this IList<TItem> list, Func<TItem, int> valueDelegate)
        {
            int result = 0;
            for (int i = 0; i < list.Count; i++) result += valueDelegate.Invoke(list[i]);
            return result;
        }

        /// <summary>
        /// Get a value from this 2-dimensional jagged list, returning a fallback value in the case that the specified indices are
        /// out-of bounds
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="lists"></param>
        /// <param name="i">The index in the outer list</param>
        /// <param name="j">The index in the inner list</param>
        /// <param name="fallback">The value to be returned when the specified index doesn't exist in this dataset</param>
        /// <returns></returns>
        public static TItem GetSafe<TItem>(this IList<IList<TItem>> lists, int i, int j, TItem fallback = default(TItem))
        {
            if (i >= 0 && i < lists.Count)
            {
                if (j >= 0 && j < lists[i].Count) return lists[i][j];
            }
            return fallback;
        }

        /// <summary>
        /// Get a random item from this list.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="rng">The random number generator to use to determine the item to be selected</param>
        /// <returns></returns>
        public static TItem GetRandom<TItem>(this IList<TItem> list, Random rng)
        {
            int i = rng.Next(0, list.Count);
            return list[i];
        }

        /// <summary>
        /// Set the value at the specified index if the list is long enough or if it is not
        /// extend the list up to that length, populating intervening indices with the
        /// default value of the appropriate type.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public static void SetAutoExtend<TItem>(this IList<TItem> list, int index, TItem item)
        {
            if (list.Count > index) list[index] = item;
            else
            { 
                while (list.Count < index)
                {
                    list.Add(default(TItem));
                }
                list.Add(item);
            }
        }

        /// <summary>
        /// Randomly change the order of the objects in this list.
        /// Uses the Fisher-Yates shuffle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="rng"></param>
        public static void Shuffle<T>(this IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
