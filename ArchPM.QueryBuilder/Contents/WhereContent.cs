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
        public WhereContent()
        {
            this.Items = new List<IContentItem>();
        }
        public List<IContentItem> Items { get; set; }

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
