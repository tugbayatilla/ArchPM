using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchPM.Core.Extensions;

namespace ArchPM.QueryBuilder.Contents
{
    public class InsertContent
    {
        public InsertContent()
        {
            this.Fields = new List<PropertyDTO>();
        }
        public List<PropertyDTO> Fields { get; set; }
        public TableInfo TableInfo { get; set; }
        public Boolean ReturningScopeIdentity { get; set; }
    }
}
