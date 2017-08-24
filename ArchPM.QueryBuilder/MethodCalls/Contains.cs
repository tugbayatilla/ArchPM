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
    class Contains : IMethodCall
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get { return "Contains"; } }

        /// <summary>
        /// Handles the specified decision maker.
        /// </summary>
        /// <param name="DecisionMaker">The decision maker.</param>
        /// <param name="sb">The sb.</param>
        /// <param name="expression">The expression.</param>
        public void Handle(DecisionMaker DecisionMaker, List<IContentItem> sb, MethodCallExpression expression)
        {
            //get not expression and remove it
            NotContentItem notItem = sb.FindLast(p => p is NotContentItem) as NotContentItem;
            sb.Remove(notItem);

            if (expression.Object != null && expression.Object.Type == typeof(String))
            {
                DecisionMaker.ExpressionHandle(sb, expression.Object);
                sb.Add(new OperatorContentItem(Operators.LIKE));
                DecisionMaker.ExpressionHandle(sb, expression.Arguments[0]);
                var last = sb.Last();
                last.Value = String.Format("{1}{0}{1}", last.Value, Operators.PERCENT.GetDescription());
            }
            else
            {

                if (expression.Arguments.Count == 1) //todo: bu ne durumu
                {
                    //field 
                    DecisionMaker.ExpressionHandle(sb, expression.Arguments[0]);

                    //operator
                    var operatorItem = new OperatorContentItem();
                    operatorItem.Value = (notItem != null ? "NOT " : "") + "IN";
                    sb.Add(operatorItem);

                    //value
                    sb.Add(new StartBlockContentItem());
                    DecisionMaker.ExpressionHandle(sb, expression.Object);
                    sb.Add(new EndBlockContentItem());
                }

                if (expression.Arguments.Count == 2) //bu ne durumu
                {
                    var fieldExpression = expression.Arguments.FirstOrDefault(p => p.Type.IsDotNetPirimitive());
                    DecisionMaker.ExpressionHandle(sb, fieldExpression);

                    var operatorItem = new OperatorContentItem();
                    operatorItem.Value = (notItem != null ? "NOT " : "") + "IN";
                    sb.Add(operatorItem);


                    sb.Add(new StartBlockContentItem());
                    var valueExpression = expression.Arguments.FirstOrDefault(p => p.Type.IsList());
                    DecisionMaker.ExpressionHandle(sb, valueExpression);
                    sb.Add(new EndBlockContentItem());
                }
            }
        }
    }
}
