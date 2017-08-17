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
        public JoinContent()
        {
            this.Items = new List<IContentItem>();
        }

        public List<IContentItem> Items { get; set; }
        public TableInfo LeftTableInfo { get; set; }
        public TableInfo RightTableInfo { get; set; }
        public JoinTypes JoinType { get; set; }
        public JoinDirections JoinDirection { get; set; }
    }
}
