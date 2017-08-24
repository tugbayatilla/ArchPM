using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Contents
{
    public class WhereContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhereContent"/> class.
        /// </summary>
        public WhereContent()
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Items)
            {
                if (item is FieldContentItem)
                {
                    var field = item as FieldContentItem;
                    sb.AppendFormat("{0}.{1}", field.TableInfo.Alias, item.Value ?? "NULL");
                    if(!String.IsNullOrEmpty( field.TableInfo.Alias))
                    {
                        sb.AppendFormat(" AS {0}", field.Alias);
                    }
                }
                else
                {
                    sb.AppendFormat("{0}", item.Value ?? "NULL");
                }
            }

            return sb.ToString();

        }
    }
}
