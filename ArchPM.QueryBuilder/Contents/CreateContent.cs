using ArchPM.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.Contents
{
    public class CreateContent
    {
        public CreateContent()
        {
            this.Fields = new List<PropertyDTO>();
        }
        public TableInfo TableInfo { get; set; }
        public List<PropertyDTO> Fields { get; set; }
    }
}
