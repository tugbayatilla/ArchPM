using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArchPM.Core.Extensions
{
    public static class AssemblyExtensions
    {
        //todo:test yazilmali
        public static IEnumerable<T> GetProvider<T>(this Assembly currentAssembly, params Object[] constructorArguments)
        {
            var types = currentAssembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsClass) continue;
                if (type.GetInterfaces().Contains(typeof(T)) || type.BaseType.Name == typeof(T).Name)
                {
                    Type result = type;
                    if (type.ContainsGenericParameters)
                    {
                        result = type.MakeGenericType(typeof(T).GenericTypeArguments);
                    }

                    var provider = (T)Activator.CreateInstance(result, constructorArguments);

                    yield return provider;
                }
            }
        }

        public static IEnumerable<Type> GetProviderTypes<T>(this Assembly currentAssembly)
        {
            var types = currentAssembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsClass) continue;
                if (type.IsAbstract) continue;
                if (type.GetInterfaces().Contains(typeof(T)) || type.BaseType.Name == typeof(T).Name)
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Create and instance and cast
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="constructorArguments"></param>
        /// <returns></returns>
        public static T CreateInstanceAndCast<T>(this Type type, params Object[] constructorArguments)
        {
            if (type.ContainsGenericParameters)
            {
                type = type.MakeGenericType(typeof(T).GenericTypeArguments);
            }
            var instance = (T)Activator.CreateInstance(type, constructorArguments);
            return instance;
        }

    }
}
