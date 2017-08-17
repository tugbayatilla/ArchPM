using System;
using System.Collections.Generic;
using ArchPM.Core.Exceptions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ArchPM.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Tests if a object is null.
        /// </summary>
        /// <typeparam name="T">class inherited from Exception</typeparam>
        /// <param name="theObj">Object class</param>
        /// <param name="msg">the message which is going to get</param>
        /// <param name="args">The arguments.</param>
        public static void NotNull<T>(this Object theObj, string msg = "", params object[] args) where T : Exception, new()
        {
            if (theObj == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    msg = "A object instance can't be null";
                }

                var ex = (T)Activator.CreateInstance(typeof(T), String.Format(msg, args));

                throw ex;

            }
        }

        /// <summary>
        /// Tests if a object is null.
        /// </summary>
        /// <typeparam name="T">class inherited from Exception</typeparam>
        /// <param name="theObj">Object class</param>
        /// <param name="msg">the message which is going to get</param>
        /// <param name="args">The arguments.</param>
        public static void NotNull(this Object theObj, string msg = "", params object[] args)
        {
            NotNull<ArgumentNullException>(theObj, msg, args);
        }

        /// <summary>
        /// Tests if provided array of objects doesnt contain null values.
        /// Throws ValidationException if it has null value.
        /// </summary>
        /// <param name="objects">array of objects</param>
        public static void NoNullElements(this object[] objects)
        {
            foreach (object obj in objects)
            {
                NotNull(obj);
            }
        }

        /// <summary>
        /// Convert a byte array to an Object
        /// </summary>
        /// <param name="arrBytes">The arr bytes.</param>
        /// <returns></returns>
        public static Object ToObject(this Byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean IsEnumOrIsBaseEnum(this Type type)
        {
            return type.IsEnum || (type.BaseType != null && type.BaseType == typeof(Enum));
        }

        /// <summary>
        /// Tests if provided bool parameter is true.
        /// Throws ValidationException with passed message if value is false.
        /// </summary>
        /// <param name="isTrue">bool value</param>
        /// <param name="msg">message to throw if the object is null</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArchPM.Core.Exceptions.ValidationException"></exception>
        public static void IsTrue(this bool isTrue, string msg, params object[] args)
        {
            if (!isTrue)
            {
                throw new ValidationException(String.Format(msg, args));
            }
        }



    }
}
