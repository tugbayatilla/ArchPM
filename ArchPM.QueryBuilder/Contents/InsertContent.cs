using ArchPM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Contents
{
    public class InsertContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertContent"/> class.
        /// </summary>
        public InsertContent()
        {
            this.Fields = new List<PropertyDTO>();
        }
        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public List<PropertyDTO> Fields { get; set; }
        /// <summary>
        /// Gets or sets the table information.
        /// </summary>
        /// <value>
        /// The table information.
        /// </value>
        public TableInfo TableInfo { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [returning scope identity].
        /// </summary>
        /// <value>
        /// <c>true</c> if [returning scope identity]; otherwise, <c>false</c>.
        /// </value>
        public Boolean ReturningScopeIdentity { get; set; }
    }
}
