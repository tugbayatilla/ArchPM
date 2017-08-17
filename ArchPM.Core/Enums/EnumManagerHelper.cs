using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArchPM.Core.Enums
{
    internal class EnumManagerHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="name"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        internal static String getDesc(Type type, String name)
        {
            String result = name;
            var attributes = type.GetField(name).GetCustomAttributes<EnumDescriptionAttribute>(false).ToList();

            //if no lang, then get first desctiption
            var attribute = attributes.FirstOrDefault();
            if (attribute != null)
            {
                result = attribute.Description;
            }

            return result;
        }
    }
}
