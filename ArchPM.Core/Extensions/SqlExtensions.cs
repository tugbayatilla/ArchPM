using ArchPM.Core.Exceptions;
using ArchPM.Core.Extensions.ObjectExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using ArchPM.Core.Extensions.ReflectionExtensions;

namespace ArchPM.Core.Extensions
{

    /// <summary>
    /// 
    /// </summary>
    public static class SqlExtensions
    {
        /// <summary>
        /// Converts the type of the SQL database.
        /// </summary>
        /// <param name="systemType">Type of the system.</param>
        /// <returns></returns>
        public static SqlDbType ToSqlDbType(this Type systemType)
        {
            return ToSqlDbType(systemType.Name);
        }

        /// <summary>
        /// Converts the type of the SQL database.
        /// </summary>
        /// <param name="systemType">Type of the system.</param>
        /// <returns></returns>
        public static SqlDbType ToSqlDbType(this String systemType)
        {
            switch (systemType)
            {
                case "System.String":
                case "String":
                case "string":
                    return SqlDbType.NVarChar;
                case "System.Int32":
                case "Int32":
                case "int":
                    return SqlDbType.Int;
                case "System.Int64":
                case "Int64":
                case "long":
                    return SqlDbType.BigInt;
                case "System.Int16":
                case "Int16":
                case "short":
                    return SqlDbType.SmallInt;
                case "System.Float":
                case "Float":
                case "float":
                    return SqlDbType.Float;
                case "System.Decimal":
                case "Decimal":
                case "decimal":
                    return SqlDbType.Decimal;
                case "System.DateTime":
                case "DateTime":
                case "datetime":
                    return SqlDbType.DateTime;
                case "System.Boolean":
                case "Boolean":
                case "bool":
                case "boolean":
                    return SqlDbType.Bit;
                case "System.Guid":
                case "Guid":
                case "guid":
                    return SqlDbType.UniqueIdentifier;

                default:
                    return SqlDbType.NVarChar;
            }
        }



        /// <summary>
        /// Converts the SQL database type as string.
        /// </summary>
        /// <param name="systemType">Type of the system.</param>
        /// <returns></returns>
        public static String ToSqlDbTypeAsString(this String systemType)
        {
            String result;
            SqlDbType sqlDbType = ToSqlDbType(systemType);

            if (sqlDbType == SqlDbType.VarChar || sqlDbType == SqlDbType.NVarChar)
                result = String.Format("{0}(max)", sqlDbType);
            else
                result = String.Format("{0}", sqlDbType);

            return result;
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
        /// Executes the reader and yields results. T1 wins
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Obsolete("use ToList2 instead!", false)]
        public static IEnumerable<T1> ToList<T1>(this IDbCommand command) where T1 : new()
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var item = new T1();
                    reader.Map(item);
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Executes the reader and yields results. Sql Wins
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static IEnumerable<T> ToList2<T>(this IDbCommand command) where T : class, new()
        {
            using (var reader = command.ExecuteReader())
            {

                var properties = typeof(T).GetType()
                    .GetProperties().ToList();

                while (reader.Read())
                {
                    T t = new T();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        String sqlFieldName = reader.GetName(i);
                        Object sqlFieldValue = reader.GetValue(i);
                        sqlFieldValue = sqlFieldValue == DBNull.Value ? null : sqlFieldValue;

                        var property = properties.FirstOrDefault(p => p.Name == sqlFieldName);
                        if (property != null)
                        {
                            if (property.IsNullable() && sqlFieldValue == null)
                                throw new DataLayerException(
                                    String.Format("Property '{0}' is not nullable in '{1}', but the sql query field '{2}' is NULL!", property.Name, typeof(T).Name, sqlFieldName));
                            if (property.PropertyType != sqlFieldValue.GetType())
                                throw new DataLayerException(
                                    String.Format(@"Property '{0}' type is '{1}' 
                                                    in '{2}', but the sql query field '{3}' 
                                                    type is '{4}'! [{1} 
                                                    {2}.{0} != {4} {3}]",
                                                    property.Name, //0
                                                    property.PropertyType.Name, //1
                                                    typeof(T).Name, //2
                                                    sqlFieldName, //3
                                                    sqlFieldValue.GetType().Name)); //4

                            property.SetValue(t, sqlFieldValue);
                        }
                    }

                    yield return t;
                }
            }
        }


        /// <summary>
        /// Dynamicly
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static IEnumerable<dynamic> ToList(this IDbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dynamic obj = new ExpandoObject();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        DynamicExtensions.AddProperty(obj, reader.GetName(i), reader.GetValue(i));
                    }

                    yield return obj;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="record"></param>
        /// <param name="entity"></param>
        public static void Map<T1>(this IDataRecord record, T1 entity)
        {
            try
            {
                var properties = entity.GetType()
                    .GetProperties().ToList();

                foreach (var property in properties)
                {
                    var dbValue = record[property.Name] == DBNull.Value ? null : record[property.Name];
                    Type type = property.PropertyType;

                    //nullable property ise, nullable altinda kalan tipi bulmaliyiz.
                    if (property.IsNullable())
                    {
                        type = Nullable.GetUnderlyingType(type);
                    }

                    //enum durumlarinda convertion yapilmali.
                    //dbValue null ise hic gerek yok parse yapmaya
                    if (dbValue != null && type.IsEnum)
                    {
                        dbValue = Enum.Parse(type, dbValue.ToString());
                    }

                    property.SetValue(entity, dbValue);
                }
            }
            catch (Exception ex)
            {
                throw new DataLayerException(String.Format("Database''den çekilen kayıtlar ile Entity ({0}) eşleştirmesinde hata ile karşılasıldı.", entity.GetType().Name), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddParameter(this IDbCommand command, String name, Object value)
        {
            command.NotNull("command");
            name.NotNull("name");

            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);
        }

    }


}
