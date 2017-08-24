using ArchPM.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.Core.Extensions
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// To the database default string.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static String ToDbDefaultString(this DateTime dt)
        {
            if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
                throw new ArgumentOutOfRangeException("dt");

            String result = String.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}",
                            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            return result;
        }

        static List<String> listNames = new List<string>() { "IEnumerable`1", "Enumerable", "List`1", "WhereSelectListIterator`2" };
        public static Boolean IsList(this Type type)
        {
            return
                (type.ReflectedType != null && listNames.Contains(type.ReflectedType.Name)
              || listNames.Contains(type.Name));
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




        /// <summary>
        /// Changes the node type to SQL operator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <example>ExpressionType.AndAlso to 'AND' or ExpressionType.Equal to '=' </example>
        public static String ToSqlOperator(this ExpressionType type)
        {
            String result = "";
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    result = "AND";
                    break;
                case ExpressionType.Equal:
                    result = "=";
                    break;
                case ExpressionType.GreaterThan:
                    result = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    result = ">=";
                    break;
                case ExpressionType.LessThan:
                    result = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    result = "<=";
                    break;
                case ExpressionType.NotEqual:
                    result = "!=";
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    result = "OR";
                    break;

                #region DEFAULTS
                case ExpressionType.Add:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AddChecked:
                case ExpressionType.AndAssign:
                case ExpressionType.ArrayIndex:
                case ExpressionType.ArrayLength:
                case ExpressionType.Assign:
                case ExpressionType.Block:
                case ExpressionType.Call:
                case ExpressionType.Coalesce:
                case ExpressionType.Conditional:
                case ExpressionType.Constant:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.DebugInfo:
                case ExpressionType.Decrement:
                case ExpressionType.Default:
                case ExpressionType.Divide:
                case ExpressionType.DivideAssign:
                case ExpressionType.Dynamic:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.Extension:
                case ExpressionType.Goto:
                case ExpressionType.Increment:
                case ExpressionType.Index:
                case ExpressionType.Invoke:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.Label:
                case ExpressionType.Lambda:
                case ExpressionType.LeftShift:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ListInit:
                case ExpressionType.Loop:
                case ExpressionType.MemberAccess:
                case ExpressionType.MemberInit:
                case ExpressionType.Modulo:
                case ExpressionType.ModuloAssign:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.New:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                case ExpressionType.Not:
                case ExpressionType.OnesComplement:
                case ExpressionType.OrAssign:
                case ExpressionType.Parameter:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.Power:
                case ExpressionType.PowerAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Quote:
                case ExpressionType.RightShift:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.RuntimeVariables:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Switch:
                case ExpressionType.Throw:
                case ExpressionType.Try:
                case ExpressionType.TypeAs:
                case ExpressionType.TypeEqual:
                case ExpressionType.TypeIs:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Unbox:
                default:
                    break;
                #endregion

            }

            return result;
        }


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
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean IsEnumOrIsBaseEnum(this Type type)
        {
            return type.IsEnum || (type.BaseType != null && type.BaseType == typeof(Enum));
        }


        /// <summary>
        /// Finds the key by value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">the value is not found in the dictionary</exception>
        public static TKey FindKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            dictionary.NotNull();

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
                if (value.Equals(pair.Value))
                    return pair.Key;

            throw new Exception("the value is not found in the dictionary");
        }

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
                result = EnumManager.GetDescription(type, name);
            }
            return result;
        }


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
            T previosItem = previousIndex >= 0 ? list[previousIndex] : null;
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



        /// <summary>
        /// Returns the number of milliseconds since Jan 1, 1970
        /// This method can be used for Json Datetime
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static String RecursiveMessage(this Exception ex, Boolean showMessageTypeAsHeader = true)
        {
            String message = "";
            if (showMessageTypeAsHeader)
                message = String.Format("[{0}]:", ex.GetType().Name);

            message += ex.Message;

            if (ex.InnerException != null)
                message += String.Concat("\r\n ", RecursiveMessage(ex.InnerException, showMessageTypeAsHeader));

            return message;
        }

        public static IEnumerable<Exception> RecursiveExceptions(this Exception ex)
        {
            if (ex != null)
                yield return ex;
            if (ex.InnerException != null)
            {
                foreach (var item in RecursiveExceptions(ex.InnerException))
                {
                    yield return item;
                }

            }

            yield break;
        }

        /// <summary>
        /// Converts Turkish to English String
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static String ToEnglish(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            #region Turkish to English Literals

            var list = new Dictionary<Char, Char>();
            list.Add('ç', 'c');
            list.Add('ö', 'o');
            list.Add('ş', 's');
            list.Add('ı', 'i');
            list.Add('ğ', 'g');
            list.Add('ü', 'u');
            list.Add('Ç', 'C');
            list.Add('Ö', 'O');
            list.Add('Ş', 'S');
            list.Add('İ', 'I');
            list.Add('Ğ', 'G');
            list.Add('Ü', 'U');

            #endregion

            text.ToList().ForEach(c =>
            {
                if (list.ContainsKey(c))
                {
                    text = text.Replace(c, list[c]);
                }
            });

            return text;
        }

    }


    public static class Extend<From>
           where From : class, new()
    {
        /// <summary>
        /// Gets the name of the property information by property.
        /// </summary>
        /// <typeparam name="From">The type of the rom.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfoByPropertyName(String propertyName)
        {
            From fromObject = new From();

            Func<String, IEnumerable<String>> createPossiles = p =>
            {
                var createPossilesResult = new List<String>();
                createPossilesResult.Add(p);
                var preparedPropertyName = p.Replace("_", "");

                createPossilesResult.Add(preparedPropertyName.ToLower());
                createPossilesResult.Add(preparedPropertyName.ToLower().ToEnglish());
                createPossilesResult.Add(preparedPropertyName.ToLowerInvariant().ToEnglish());
                createPossilesResult = createPossilesResult.Distinct().ToList();

                return createPossilesResult;
            };

            //prepare possible names
            var searchNames = new List<String>();
            searchNames.AddRange(createPossiles(propertyName));

            var result = fromObject.GetType().GetProperties().FirstOrDefault(p => createPossiles(p.Name).Intersect(searchNames).Any());

            return result;
        }
    }

}
