using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder
{
    sealed class TableInfoContainer
    {
        List<TableInfo> list;
        public IReadOnlyCollection<TableInfo> List { get { return list; } }

        public TableInfoContainer()
        {
            this.list = new List<TableInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="alias"></param>
        /// <exception cref="QueryBuilderException">'{0}' is already changed to '{1}'</exception>
        public TableInfo SetAlias(String tableName, String alias = "")
        {
            var table = this.List.FirstOrDefault(p => p.Name == tableName || p.OldName == tableName);
            if (table == null) //not exist
            {
                alias = String.IsNullOrEmpty(alias) ? String.Format("t{0}", this.List.Count) : alias;

                table = new TableInfo() { Name = tableName, Alias = alias };
                this.list.Add(table);
            }
            else //table exist
            {
                if (!String.IsNullOrEmpty(alias))
                {
                    table.Alias = alias;
                }
            }

            return table;
        }

        /// <summary>
        /// Gets the specified table name. if it does not exist, calls SetAlias method with empty alias 
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public TableInfo Get(String tableName)
        {
            var table = this.List.FirstOrDefault(p => p.Name == tableName || p.OldName == tableName);
            if (table == null)
            {
                table = SetAlias(tableName, "");
            }

            return table;
        }

        public void ChangeName(String oldName, String newName)
        {
            var table = this.List.FirstOrDefault(p => p.Name == oldName);
            if (table != null)
            {
                table.OldName = oldName;
                table.Name = newName;
            }

        }

    }
}
