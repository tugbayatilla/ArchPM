using ArchPM.Core.Enums;
using System;
using System.Collections.Generic;

namespace ArchPM.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static String GetDescription(this Enum obj)
        {
            String result = String.Empty;
            if (obj != null)
            {
                Type type = obj.GetType();
                String name = obj.ToString();
                result = EnumManagerHelper.getDesc(type, name);
            }
            return result;
        }

        /// <summary>
        /// Get value as String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String GetValueAsString(this Enum obj)
        {
            Type type = obj.GetType();

            return Convert.ToString((Int32)type.GetField(obj.ToString()).GetValue(obj));
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static String GetName(this Enum obj)
        {
            return Enum.GetName(obj.GetType(), obj);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Int32 GetValue(this Enum obj)
        {
            return Convert.ToInt32(obj);
        }


        /// <summary>
        /// Iterates all enum items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="action">The action.</param>
        public static void Foreach<T>(this Enum obj, Action<T> action)
        {
            var enumNames = Enum.GetNames(typeof(T));

            foreach (var enumName in enumNames)
            {
                T enumRole = (T)Enum.Parse(typeof(T), enumName);
                action(enumRole);
            }
        }

        /// <summary>
        /// To the array.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static IEnumerable<Enum> ToArray(this Enum obj)
        {
            foreach (Enum value in Enum.GetValues(obj.GetType()))
                if (obj.HasFlag(value))
                    yield return value;
        }

        /// <summary>
        /// Determines whether [has] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean Has<T>(this System.Enum type, T value)
        {
            try
            {
                return (((Int32)(Object)type & (Int32)(Object)value) == (Int32)(Object)value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean Is<T>(this System.Enum type, T value)
        {
            try
            {
                return (Int32)(Object)type == (Int32)(Object)value;
            }
            catch
            {
                return false;
            }
        }


    }
}
