using System;
using ArchPM.Core.Extensions;

namespace ArchPM.QueryBuilder.ContentItems
{
    /// <summary>
    /// Represent something made of two things or parts specifically
    /// </summary>
    /// <seealso cref="ArchPM.QueryBuilder.IContentItem" />
    public class OperatorContentItem : IContentItem
    {
        public OperatorContentItem()
        {

        }

        public OperatorContentItem(Operators ops)
        {
            this.Value = ops.GetDescription();
        }

        public Object Value { get; set; }
        public String ValueStr { get { return Value as String; } set { this.Value = value; } }
    }

}
