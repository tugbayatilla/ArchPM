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
        public SelectContent()
        {
            this.Items = new List<IContentItem>();
        }
        public TableInfo TableInfo { get; set; }
        public List<IContentItem> Items { get; set; }
        public Boolean IsUsingCountAggreate { get; set; }
    }
}
