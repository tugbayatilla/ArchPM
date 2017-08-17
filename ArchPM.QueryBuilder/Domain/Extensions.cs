//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ArchPM.Domain
//{
//    public static class Extensions
//    {
//        /// <summary>
//        /// SqlQueryBuilder Extension. Convert IQueryString list to string
//        /// </summary>
//        /// <param name="queryString"></param>
//        /// <returns></returns>
//        internal static String ToQueryString(this List<IQueryString> queryString, String seperator = "", Boolean withAlias = true)
//        {
//            List<String> list = new List<String>();

//            foreach (var item in queryString)
//            {
//                if (item.Type == QueryStringTypes.Field && withAlias)
//                {
//                    list.Add(String.Format("{0}.{1}", item.TableAlias, item.Text));
//                }
//                else
//                {
//                    list.Add(item.Text);
//                }


//            }
//            return String.Join(seperator, list);
//        }


//        public static Expression<Func<T, Boolean>> AndAlso<T>(this Expression<Func<T, Boolean>> firstExp, Expression<Func<T, Boolean>> secondExp)
//        {
//            ParameterExpression param = firstExp.Parameters[0];
//            ParameterExpression param2 = secondExp.Parameters[0];

//            // simple version
//            if (ReferenceEquals(param, param2))
//            {
//                return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(firstExp.Body, secondExp.Body), param);
//            }

//            // otherwise, keep expr1 "as is" and invoke expr2
//            Expression body = Expression.AndAlso(firstExp.Body, Expression.Invoke(secondExp, param));
//            return Expression.Lambda<Func<T, bool>>(body, param);
//        }

//        public static Expression<Func<T, U, Boolean>> AndAlso<T, U>(this Expression<Func<T, U, Boolean>> firstExp, Expression<Func<T, U, Boolean>> secondExp)
//        {
//            ParameterExpression param = firstExp.Parameters[0];
//            ParameterExpression param2 = secondExp.Parameters[0];

//            // simple version
//            if (ReferenceEquals(param, param2))
//            {
//                return Expression.Lambda<Func<T, U, bool>>(Expression.AndAlso(firstExp.Body, secondExp.Body), param);
//            }

//            // otherwise, keep expr1 "as is" and invoke expr2
//            Expression body = Expression.AndAlso(firstExp.Body, Expression.Invoke(secondExp, param));
//            return Expression.Lambda<Func<T, U, bool>>(body, param);
//        }

//        public static Expression<Func<T, Boolean>> OrElse<T>(this Expression<Func<T, Boolean>> firstExp, Expression<Func<T, Boolean>> secondExp)
//        {
//            ParameterExpression param = firstExp.Parameters[0];
//            ParameterExpression param2 = secondExp.Parameters[0];

//            // simple version
//            if (ReferenceEquals(param, param2))
//            {
//                return Expression.Lambda<Func<T, bool>>(Expression.Block(Expression.OrElse(firstExp.Body, secondExp.Body)), param);
//            }

//            // otherwise, keep expr1 "as is" and invoke expr2
//            Expression body = Expression.Block(Expression.OrElse(firstExp.Body, Expression.Invoke(secondExp, param)));
//            return Expression.Lambda<Func<T, bool>>(body, param);
//        }

//        public static Expression<Func<T, U, Boolean>> OrElse<T, U>(this Expression<Func<T, U, Boolean>> firstExp, Expression<Func<T, U, Boolean>> secondExp)
//        {
//            ParameterExpression param = firstExp.Parameters[0];
//            ParameterExpression param2 = secondExp.Parameters[0];

//            // simple version
//            if (ReferenceEquals(param, param2))
//            {
//                return Expression.Lambda<Func<T, U, bool>>(Expression.Block(Expression.OrElse(firstExp.Body, secondExp.Body)), param);
//            }

//            // otherwise, keep expr1 "as is" and invoke expr2
//            Expression body = Expression.Block(Expression.OrElse(firstExp.Body, Expression.Invoke(secondExp, param)));
//            return Expression.Lambda<Func<T, U, bool>>(body, param);
//        }


//        //internal static void Remove(this List<IQueryString> sb, Predicate<IQueryString> predicate)
//        //{
//        //    if (sb.Any(p => p.Type == QueryStringTypes.RemoveMe))
//        //    {
//        //        Int32 index = sb.FindIndex(p => p.Type == QueryStringTypes.RemoveMe);
//        //        sb.RemoveAt(index);
//        //    }
//        //}

//    }
//}
