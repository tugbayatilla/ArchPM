using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.MethodCalls
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMethodCall
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String Name { get; }

        /// <summary>
        /// Handles the specified decision maker.
        /// </summary>
        /// <param name="DecisionMaker">The decision maker.</param>
        /// <param name="sb">The sb.</param>
        /// <param name="expression">The expression.</param>
        void Handle(DecisionMaker DecisionMaker, List<IContentItem> sb, MethodCallExpression expression);
    }
}
