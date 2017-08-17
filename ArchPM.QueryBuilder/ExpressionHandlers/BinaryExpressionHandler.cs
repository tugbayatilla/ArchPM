using System;
using System.Linq.Expressions;
using System.Text;
using ArchPM.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    class BinaryExpressionHandler : IExpressionHandler
    {
        public DecisionMaker DecisionMaker { get; set; }
        public Type Type { get { return typeof(BinaryExpression); } }

        public void Handle(List<IContentItem> sb, Expression expression)
        {
            BinaryExpression binaryExp = expression as BinaryExpression;
            if (binaryExp == null)
                return;
            //
            if (IsContainsBlock(binaryExp))
            {
                sb.Add(new StartBlockContentItem());
            }

            DecisionMaker.ExpressionHandle(sb, binaryExp.Left);
            sb.Add(new OperatorContentItem() { Value = binaryExp.NodeType.ToSqlOperator() });
            DecisionMaker.ExpressionHandle(sb, binaryExp.Right);

            if (IsContainsBlock(binaryExp))
            {
                sb.Add(new EndBlockContentItem());
            }
        }

        internal Boolean IsContainsBlock(BinaryExpression exp)
        {
            return (exp.NodeType == ExpressionType.AndAlso || exp.NodeType == ExpressionType.OrElse)
                && (exp.Left is BinaryExpression || exp.Right is BinaryExpression);
        }
    }
}
