using System;

namespace ArchPM.Core.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RepositoryInfoAttribute : Attribute
    {
        /// <summary>
        /// Get RepositoryType  
        /// </summary>
        /// <value>
        /// The name of the Type.
        /// </value>
        public RepositoryTypes RepositoryType { get; set; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public ApplicationDataTypes DataType { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryInfoAttribute"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public RepositoryInfoAttribute(RepositoryTypes repositoryType, ApplicationDataTypes dataType)
        {
            this.RepositoryType = repositoryType;
            this.DataType = dataType;
        }

    }



}