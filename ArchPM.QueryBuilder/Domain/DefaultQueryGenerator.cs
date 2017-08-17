using ArchPM.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM
{
    public class DefaultQueryGenerator : IQueryGenerator
    {
        public string Execute(QBuilder builder)
        {
            return "DefaultQueryGenerator";
        }

        public bool UseStar { get; set; }

        public bool Pretty { get; set; }
    }
}
