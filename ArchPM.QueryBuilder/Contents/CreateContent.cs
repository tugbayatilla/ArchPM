using ArchPM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateContent"/> class.
        /// </summary>
        public CreateContent()
        {
            this.Fields = new List<PropertyDTO>();
        }
        /// <summary>
        /// Gets or sets the table information.
        /// </summary>
        /// <value>
        /// The table information.
        /// </value>
        public TableInfo TableInfo { get; set; }
        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public List<PropertyDTO> Fields { get; set; }
    }
}
