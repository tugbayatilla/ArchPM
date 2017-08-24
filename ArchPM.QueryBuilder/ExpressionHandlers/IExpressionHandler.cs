using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    public interface IExpressionHandler
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        Type Type { get; }
        /// <summary>
        /// Gets or sets the decision maker.
        /// </summary>
        /// <value>
        /// The decision maker.
        /// </value>
        DecisionMaker DecisionMaker { get; set; }
        /// <summary>
        /// Handles the specified sb.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="expression">The expression.</param>
        void Handle(List<IContentItem> sb, Expression expression);
    }
}
