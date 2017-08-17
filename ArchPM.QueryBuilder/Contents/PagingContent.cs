using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Contents
{
    public class PagingContent
    {
        public PagingContent()
        {
            this.Items = new List<IContentItem>();
        }
        public List<IContentItem> Items { get; set; }
        public Int32 Page { get; set; }
        public Int32 PerPage { get; set; }
        public TableInfo TableInfo { get; set; }
        public OrderByDirections Direction { get; set; }
    }
}
