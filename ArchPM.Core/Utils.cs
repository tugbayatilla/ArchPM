using ArchPM.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.Core
{
    public static class Utils
    {
        /// <summary>
        /// Creates the unique number.
        /// </summary>
        /// <returns></returns>
        public static UInt64 CreateUniqueNumber()
        {
            var unique = String.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
            var result = Convert.ToUInt64(unique);

            return result;
        }

        public static PropertyInfo FindProperty<From>(String searchName)
            where From : class, new()
        {
            From fromObject = new From();

            Func<String, IEnumerable<String>> createPossiles = p => {
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
            searchNames.AddRange(createPossiles(searchName));

            var result = fromObject.GetType().GetProperties().FirstOrDefault(p => createPossiles(p.Name).Intersect(searchNames).Any());

            return result;
        }


    }
}
