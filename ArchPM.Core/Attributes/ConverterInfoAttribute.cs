using System;

namespace ArchPM.Core.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ConverterInfoAttribute : Attribute
    {
        /// <summary>
        /// Gets and Sets From Type
        /// </summary>
        public Type FromType { get; set; }

        /// <summary>
        /// Gets and Sets To Type
        /// </summary>
        public Type ToType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterInfoAttribute"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public ConverterInfoAttribute(Type fromType, Type toType)
        {
            this.FromType = fromType;
            this.ToType = toType;
        }

    }

   

}