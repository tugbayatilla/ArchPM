using System;

namespace ArchPM.QueryBuilder.ContentItems
{
    /// <summary>
    /// Represent something made of two things or parts specifically
    /// </summary>
    /// <seealso cref="ArchPM.QueryBuilder.IContentItem" />
    public abstract class BlockContentItem : IContentItem
    {
        public Object Value { get; set; }
        public String ValueStr { get { return Value as String; } set { this.Value = value; } }
    }

    public sealed class StartBlockContentItem : BlockContentItem
    {
        public StartBlockContentItem()
        {
            this.Value = "(";
        }
    }

    public sealed class EndBlockContentItem : BlockContentItem
    {
        public EndBlockContentItem()
        {
            this.Value = ")";
        }
    }

}
