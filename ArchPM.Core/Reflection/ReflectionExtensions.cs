using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

using System.Linq;
using ArchPM.Core.Extensions.TypeExtensions;
using ArchPM.Core.Extensions.ObjectExtensions;

namespace ArchPM.Core.Extensions.ReflectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionExtensions
    {

        public static void FillDummyData<T>(this T entity) where T : class, new()
        {
            if (entity == null)
                return;

            entity.Properties().ForEach(p =>
            {
                Object value = null;
                switch (p.ValueType)
                {
                    case "String":
                        value = "test";
                        break;
                    case "Int32":
                        value = 99;
                        break;
                    case "Int64":
                        value = Convert.ToInt64(99);
                        break;
                    case "Int16":
                        value = Convert.ToInt16(99);
                        break;
                    case "Float":
                        value = Convert.ToInt16(99);
                        break;
                    case "Decimal":
                        value = Convert.ToDecimal(99);
                        break;
                    case "DateTime":
                        value = new DateTime(2000, 1, 1);
                        break;
                    case "Boolean":
                        value = true;
                        break;
                    case "Guid":
                        value = Guid.Empty;
                        break;
                    default:
                        break;
                }

                if (p.IsEnum)
                {
                    var values = Enum.GetValues(p.ValueTypeOf);
                    if (values.Length > 0)
                        value = values.GetValue(0);
                }

                if (value != null)
                {
                    var prop = entity.GetType().GetProperty(p.Name);
                    if(prop.CanWrite)
                        prop.SetValue(entity, value);
                }

            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfosByAttributes<T>(this T entity, params Type[] attributeTypes)
        {
            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.GetCustomAttributes().Any(p => attributeTypes.Contains(p.GetType())))
                    yield return property;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="attributeTypes"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyDTO> GetPropertiesByAttributes<T>(this T entity, params Type[] attributeTypes)
        {
            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.GetCustomAttributes().Any(p => attributeTypes.Contains(p.GetType())))
                    yield return convertPropertyInfoToPropertyDTO(entity, property);
            }
        }

        /// <summary>
        /// Entities the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public static IEnumerable<PropertyDTO> Properties<T>(this T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            PropertyInfo[] properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                //ignore if list, interface, class
                if (property.PropertyType.IsList()
                 || property.PropertyType.IsInterface
                 || (property.PropertyType != typeof(String) && property.PropertyType.IsClass))
                {
                    continue;
                }

                var entityProperty = convertPropertyInfoToPropertyDTO(entity, property);

                yield return entityProperty;
            }
        }

        internal static PropertyDTO convertPropertyInfoToPropertyDTO<T>(T entity, PropertyInfo property)
        {
            var entityProperty = new PropertyDTO();

            entityProperty.Name = property.Name;
            try
            {
                entityProperty.Value = property.GetValue(entity, null);
            }
            catch
            {
                entityProperty.Value = null;
            }
            entityProperty.ValueType = property.PropertyType.Name;
            entityProperty.ValueTypeOf = property.PropertyType;
            entityProperty.Nullable = false;

            if (property.IsNullable())
            {
                entityProperty.ValueType = Nullable.GetUnderlyingType(property.PropertyType).Name;
                entityProperty.ValueTypeOf = Nullable.GetUnderlyingType(property.PropertyType);
                entityProperty.Nullable = true;
            }

            //when datetime gets the default value
            if (entityProperty.Value != null && entityProperty.ValueTypeOf == typeof(DateTime) && (DateTime)entityProperty.Value == default(DateTime))
            {
                entityProperty.Value = null;
                entityProperty.Nullable = true;
            }
            //String is reference type, it can be null
            if (entityProperty.ValueTypeOf == typeof(String))
            {
                entityProperty.Nullable = true;
            }
            entityProperty.IsPrimitive = entityProperty.ValueTypeOf.IsDotNetPirimitive();
            entityProperty.IsEnum = entityProperty.ValueTypeOf.IsEnumOrIsBaseEnum();

            return entityProperty;
        }

        public static Boolean IsNullable(this PropertyInfo property)
        {
            return property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        /// <summary>
        /// Convert Enum Value to Int32 Value
        /// </summary>
        /// <param name="property">The property.</param>
        public static void ConvertEnumValueToInt32OnPropertyValue(this PropertyDTO property)
        {
            //var value = property.Value == null ? DBNull.Value : property.Value;
            var value = property.Value;
            if (value != null && property.ValueTypeOf.IsEnum)
            {
                value = Convert.ToInt32(property.Value);
            }

            property.Value = value;
        }
    }
}
