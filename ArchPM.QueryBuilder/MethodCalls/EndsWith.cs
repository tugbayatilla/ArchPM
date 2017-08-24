using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using ArchPM.Core.Extensions;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder.MethodCalls
{
    class EndsWith : IMethodCall
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get { return "EndsWith"; } }

        /// <summary>
        /// Handles the specified decision maker.
        /// </summary>
        /// <param name="DecisionMaker">The decision maker.</param>
        /// <param name="sb">The sb.</param>
        /// <param name="expression">The expression.</param>
        public void Handle(DecisionMaker DecisionMaker, List<IContentItem> sb, MethodCallExpression expression)
        {
            DecisionMaker.ExpressionHandle(sb, expression.Object);
            sb.Add(new OperatorContentItem(Operators.LIKE));
            DecisionMaker.ExpressionHandle(sb, expression.Arguments[0]);
            var last = sb.Last();
            last.Value = String.Format("{1}{0}", last.Value, Operators.PERCENT.GetDescription());
        }
    }
}
