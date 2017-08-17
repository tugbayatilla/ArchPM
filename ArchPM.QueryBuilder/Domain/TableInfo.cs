using ArchPM.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder
{
    public class TableInfo
    {
        public TableInfo()
        {
            this.OldName = "";
        }

        public String Name { get; set; }
        public String OldName { get; set; }
        public String Alias { get; set; }

    }

    
}
