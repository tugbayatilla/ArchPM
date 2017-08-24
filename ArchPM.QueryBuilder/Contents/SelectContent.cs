using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Contents
{
    public class SelectContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectContent"/> class.
        /// </summary>
        public SelectContent()
        {
            this.Items = new List<IContentItem>();
        }
        /// <summary>
        /// Gets or sets the table information.
        /// </summary>
        /// <value>
        /// The table information.
        /// </value>
        public TableInfo TableInfo { get; set; }
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<IContentItem> Items { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is using count aggreate.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is using count aggreate; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsUsingCountAggreate { get; set; }
    }
}
