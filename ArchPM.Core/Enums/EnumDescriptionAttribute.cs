using System;

namespace ArchPM.Core.Enums
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumDescriptionAttribute : Attribute
    {
        /// <summary>
        /// The description
        /// </summary>
        private String description;
        private Boolean exclude = false;

        /// <summary>
        /// Get description
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets and Sets it is excluded for not
        /// </summary>
        /// <value>
        ///   <c>true</c> if exclude; otherwise, <c>false</c>.
        /// </value>
        public Boolean Exclude
        {
            get { return exclude; }
            set { exclude = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public EnumDescriptionAttribute(String description)
        {
            this.description = description;
        }

       
    }
}