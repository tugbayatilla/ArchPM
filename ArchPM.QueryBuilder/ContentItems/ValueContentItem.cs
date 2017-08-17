using System;

namespace ArchPM.QueryBuilder.ContentItems
{
    /// <summary>
    /// Represent something made of two things or parts specifically
    /// </summary>
    /// <seealso cref="ArchPM.QueryBuilder.IContentItem" />
    public class ValueContentItem : IContentItem
    {
        public Object Value { get; set; }
    }

}
