using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ArchPM.Core.Extensions.TypeExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        public static Type GetMemberUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                default:
                    throw new ArgumentException("MemberInfo must be if type FieldInfo, PropertyInfo or EventInfo", "member");
            }
        }

        static List<String> listNames = new List<string>() { "IEnumerable`1", "Enumerable", "List`1", "WhereSelectListIterator`2" };
        public static Boolean IsList(this Type type)
        {
            return
                (type.ReflectedType != null && listNames.Contains(type.ReflectedType.Name)
              || listNames.Contains(type.Name));
        }

        public static IEnumerable<FieldInfo> GetConstants(this Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
        }
        public static IEnumerable<T> GetConstantsValues<T>(this Type type) where T : class
        {
            var fieldInfos = GetConstants(type);

            return fieldInfos.Select(fi => fi.GetRawConstantValue() as T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String AnonymousSupportedTypeName(this Type type)
        {
            String name = type.Name;
            if (type.IsAnonymousType())
            {
                name = name
                    .Replace("<>", "")
                    .Replace("VB$", "")
                    .Replace("`", "");
            }

            return name;
        }

        public static Boolean IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        /// <summary>
        /// Check if it is anonymous or not
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public static Boolean IsAnonymousType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static Boolean IsDotNetPirimitive(this Type systemType)
        {
            if (systemType == typeof(String)
                || systemType == typeof(Int32)
                || systemType == typeof(Int64)
                || systemType == typeof(Int16)
                || systemType == typeof(float)
                || systemType == typeof(Decimal)
                || systemType == typeof(DateTime)
                || systemType == typeof(Boolean)
                || systemType == typeof(Guid)
                || systemType == typeof(Enum)
                || systemType.IsEnumOrIsBaseEnum())
                return true;
            else
                return false;
        }


    }

    
}
