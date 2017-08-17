using System;

namespace ArchPM.Core.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ContextInfoAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public ApplicationDataTypes DataType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextInfoAttribute"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public ContextInfoAttribute(ApplicationDataTypes dataType)
        {
            this.DataType = dataType;
        }

    }

}