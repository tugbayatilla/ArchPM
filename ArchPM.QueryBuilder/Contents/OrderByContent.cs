using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Contents
{
    public class OrderByContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByContent"/> class.
        /// </summary>
        public OrderByContent()
        {
            this.Items = new List<IContentItem>();
        }
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<IContentItem> Items { get; set; }
        /// <summary>
        /// Gets or sets the table information.
        /// </summary>
        /// <value>
        /// The table information.
        /// </value>
        public TableInfo TableInfo { get; set; }
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public OrderByDirections Direction { get; set; }
    }
}
