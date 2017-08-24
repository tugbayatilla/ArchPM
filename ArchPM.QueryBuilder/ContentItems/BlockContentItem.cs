using System;

namespace ArchPM.QueryBuilder.ContentItems
{
    /// <summary>
    /// Represent something made of two things or parts specifically
    /// </summary>
    /// <seealso cref="ArchPM.QueryBuilder.ContentItems.IContentItem" />
    /// <seealso cref="ArchPM.QueryBuilder.IContentItem" />
    public abstract class BlockContentItem : IContentItem
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Object Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ArchPM.QueryBuilder.ContentItems.BlockContentItem" />
    public sealed class StartBlockContentItem : BlockContentItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartBlockContentItem"/> class.
        /// </summary>
        public StartBlockContentItem()
        {
            this.Value = "(";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ArchPM.QueryBuilder.ContentItems.BlockContentItem" />
    public sealed class EndBlockContentItem : BlockContentItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndBlockContentItem"/> class.
        /// </summary>
        public EndBlockContentItem()
        {
            this.Value = ")";
        }
    }

}
