using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder
{
    public interface IQueryGenerator
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use star].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use star]; otherwise, <c>false</c>.
        /// </value>
        Boolean UseStar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IQueryGenerator"/> is showing query as pretty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if pretty; otherwise, <c>false</c>.
        /// </value>
        Boolean Pretty { get; set; }

        /// <summary>
        /// Executes the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        String Execute(QBuilder builder);
    }
}
