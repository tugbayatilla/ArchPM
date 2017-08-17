using System;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    class UnaryExpressionHandler : IExpressionHandler
    {
        public DecisionMaker DecisionMaker { get; set; }
        public Type Type { get { return typeof(UnaryExpression); } }

        public void Handle(List<IContentItem> sb, Expression expression)
        {
            UnaryExpression unaryExp = expression as UnaryExpression;
            if (unaryExp == null)
                return;

            if (unaryExp.NodeType == ExpressionType.Not)
            {
                sb.Add(new NotContentItem());
            }

            DecisionMaker.ExpressionHandle(sb, unaryExp.Operand);
        }
    }
}
