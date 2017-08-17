using System;

namespace ArchPM.QueryBuilder.ContentItems
{
    /// <summary>
    /// Represent single item or element
    /// </summary>
    /// <seealso cref="ArchPM.QueryBuilder.IContentItem" />
    public class FieldContentItem : IContentItem
    {
        public FieldContentItem()
        {
            this.TableInfo = new TableInfo();
        }
        public Object Value { get; set; }
        public String ValueStr { get { return Value as String; } set { this.Value = value; } }
        public Type Type { get; set; }
        public TableInfo TableInfo { get; set; }
        public String Alias { get; set; }
    }

}
