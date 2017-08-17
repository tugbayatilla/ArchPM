using ArchPM.Core.Exceptions;
using ArchPM.Core.Extensions.ObjectExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using ArchPM.Core.Extensions.ReflectionExtensions;

namespace ArchPM.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="projection"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="projection"></param>
        public static void ModifyEach<T>(this IList<T> source, Func<T, T> projection)
        {
            for (int i = 0; i < source.Count; i++)
            {
                source[i] = projection(source[i]);
            }
        }

        public static T PreviousItemIfExist<T>(this IList<T> list, T currentItem) where T : class
        {
            Int32 currentIndex = list.IndexOf(currentItem);
            Int32 previousIndex = currentIndex - 1;
            T previosItem =  previousIndex >= 0 ? list[previousIndex] : null;
            return previosItem;
        }

        public static T NextItemIfExist<T>(this IList<T> list, T currentItem) where T : class
        {
            Int32 currentIndex = list.IndexOf(currentItem);
            Int32 nextIndex = currentIndex + 1;
            T previosItem = list.Count > nextIndex ? list[nextIndex] : null;
            return previosItem;
        }


        /// <summary>
        /// Tests if provided list of objects doesnt contain null values.
        /// Throws ValidationException if it has null value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">list of objects</param>
        public static void NoNullElements<T>(this IList<T> objects)
        {
            foreach (object obj in objects)
            {
                obj.NotNull();
            }
        }

        /// <summary>
        /// Tests if provided list of objects is not null or empty.
        /// Throws ValidationException if it is null or is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">list of objects</param>
        /// <exception cref="ArchPM.Core.Exceptions.ValidationException">The list can't be empty</exception>
        public static void NotEmpty<T>(this IList<T> objects)
        {
            if (objects == null || objects.Count == 0)
            {
                throw new ValidationException("The list can't be empty");
            }
        }

        /// <summary>
        /// Tests if provided list of objects is not null or empty.
        /// Throws ValidationException if it is null or is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">list of objects</param>
        /// <param name="msg">The MSG.</param>
        /// <exception cref="ArchPM.Core.Exceptions.ValidationException"></exception>
        public static void NotEmpty<T>(this IList<T> objects, string msg)
        {
            if (objects == null || objects.Count == 0)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    msg = "The list can't be empty";
                }
                throw new ValidationException(msg);
            }
        }


        /// <summary>
        /// Adds as unique.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        public static void AddAsUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Check list and records. if list is not null and contains records, returns true, otherwise returns false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">List of T</param>
        /// <returns></returns>
        public static Boolean HasRecords<T>(this IEnumerable<T> list)
        {
            Boolean result = false;

            if (list != null)
            {
                result = list.Count() > 0;
            }

            return result;
        }

        /// <summary>
        /// To the data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items) where T : class
        {
            items.NotNull();
            var hasRecords = items.HasRecords();
            if (!hasRecords)
            {
                throw new Exception("Need at least one record in list!");
            }

            var dataTable = new DataTable(typeof(T).Name);

            var t = items.First();
            var properties = t.Properties();

            foreach (var prop in properties)
            {
                DataColumn column = new DataColumn(prop.Name, Type.GetType("System." + prop.ValueType));
                column.AllowDBNull = prop.Nullable;
                if (prop.ValueType == "String")
                    column.AllowDBNull = true;

                dataTable.Columns.Add(column);
            }

            foreach (var item in items)
            {
                var props = item.Properties();
                var values = props.Select(p => p.Value).ToArray();
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }


    }
}
