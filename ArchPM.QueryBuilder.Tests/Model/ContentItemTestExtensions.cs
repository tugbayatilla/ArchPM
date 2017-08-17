using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Tests.Model
{
    internal static class ContentItemTestExtensions
    {
        internal static bool CheckTableInfo(this List<IContentItem> list, String name, String alias, String newName)
        {
            Boolean result = false;

            var checkList = list.Where(p => p is FieldContentItem).Cast<FieldContentItem>().Select(p => p.TableInfo);

            if (String.IsNullOrEmpty(newName))
            {
                result = checkList.Any(item => item.Name == name && item.Alias == alias);
            }
            else
            {
                result = checkList.Any(item => item.Name == newName && item.Alias == alias && item.OldName == name);
            }

            return result;
        }

        internal static Boolean CheckTableInfo(this TableInfo info, String name, String alias, String newName)
        {
            Boolean result = false;
            if (String.IsNullOrEmpty(newName))
            {
                result = info.Alias == alias && info.Name == name;
            }
            else
            {
                result = info.Alias == alias && info.OldName == name && info.Name == newName;
            }


            return result;
        }

        /// <summary>
        /// Item1: Boolean: All items are are equal and same order
        /// Item2: Object: If False, Problematic object returns, otherwise return Empty.String
        /// Item3: Int32: Number of parameters
        /// </summary>
        /// <param name="list"></param>
        /// <param name="values"></param>
        /// <returns>
        /// Item1: Boolean: All items are are equal and same order
        /// Item2: Object: If False, Problematic object returns, otherwise return Empty.String
        /// Item3: Int32: Number of parameters
        /// </returns>
        internal static Tuple<Boolean, Object, Int32> Check(this List<IContentItem> list, params Object[] values)
        {
            var result = new Tuple<Boolean, Object, Int32>(true, String.Empty, values.Length); ;

            if (list.Count != values.Count())
                return result;

            for (int i = 0; i < values.Length; i++)
            {
                if (!Object.Equals(list[i].Value, values[i]))
                    return new Tuple<Boolean, Object, Int32>(false, values[i], values.Length);
            }

            return result;
        }

    }
}
