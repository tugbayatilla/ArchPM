using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    class LambdaExpressionHandler : IExpressionHandler
    {
        public DecisionMaker DecisionMaker { get; set; }
        public Type Type { get { return typeof(LambdaExpression); } }

        public void Handle(List<IContentItem> sb, Expression expression)
        {
            LambdaExpression lambdaExp = expression as LambdaExpression;
            if (lambdaExp == null)
                return;

            Expression body = lambdaExp.Body;
            DecisionMaker.ExpressionHandle(sb, body);
        }
    }
}
