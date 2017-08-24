using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Contents
{
    public class JoinContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JoinContent"/> class.
        /// </summary>
        public JoinContent()
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
        /// Gets or sets the left table information.
        /// </summary>
        /// <value>
        /// The left table information.
        /// </value>
        public TableInfo LeftTableInfo { get; set; }
        /// <summary>
        /// Gets or sets the right table information.
        /// </summary>
        /// <value>
        /// The right table information.
        /// </value>
        public TableInfo RightTableInfo { get; set; }
        /// <summary>
        /// Gets or sets the type of the join.
        /// </summary>
        /// <value>
        /// The type of the join.
        /// </value>
        public JoinTypes JoinType { get; set; }
        /// <summary>
        /// Gets or sets the join direction.
        /// </summary>
        /// <value>
        /// The join direction.
        /// </value>
        public JoinDirections JoinDirection { get; set; }
    }
}
