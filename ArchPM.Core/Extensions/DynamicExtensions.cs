using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.Core.Extensions
{
    public static class DynamicExtensions
    {
        public static void AddProperty(this ExpandoObject expando, String propertyName, Object propertyValue)
        {
            var expandoDict = expando as IDictionary<String, Object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
    }
}
