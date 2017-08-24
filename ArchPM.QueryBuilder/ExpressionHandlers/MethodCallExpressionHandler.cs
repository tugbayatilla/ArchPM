using System;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using ArchPM.Core.Extensions;
using System.Collections;
using System.Linq;

using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    class MethodCallExpressionHandler : IExpressionHandler
    {
        public DecisionMaker DecisionMaker { get; set; }
        public Type Type { get { return typeof(MethodCallExpression); } }

        public void Handle(List<IContentItem> sb, Expression expression)
        {
            MethodCallExpression methodCallExp = expression as MethodCallExpression;
            if (methodCallExp == null)
                return;

            var method = DecisionMaker.registeredMethodCalls.FirstOrDefault(p => p.Name == methodCallExp.Method.Name);

            if(method != null)
            {
                method.Handle(this.DecisionMaker, sb, methodCallExp);
            }
            else
            {
                Object obj = Expression.Lambda(expression).Compile().DynamicInvoke();
                if(obj.GetType().IsDotNetPirimitive())
                {
                    ConstantExpression constantExpression = Expression.Constant(obj);
                    DecisionMaker.ExpressionHandle(sb, constantExpression);
                }
            }
        }
    }
}
