using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using ArchPM.Core.Extensions;
using System.Globalization;
using ArchPM.QueryBuilder.ExpressionHandlers;
using ArchPM.Core.Exceptions;
using ArchPM.QueryBuilder.MethodCalls;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder
{
    public sealed class DecisionMaker
    {
        readonly List<IExpressionHandler> registeredExpressions;
        internal readonly List<IMethodCall> registeredMethodCalls;
        public DecisionMaker(IEnumerable<IExpressionHandler> registeredExpressions, IEnumerable<IMethodCall> registeredMethodCalls)
        {
            this.registeredExpressions = new List<IExpressionHandler>();
            this.registeredExpressions.AddRange(registeredExpressions);

            this.registeredMethodCalls = new List<IMethodCall>();
            this.registeredMethodCalls.AddRange(registeredMethodCalls);
        }

        public void ExpressionHandle(List<IContentItem> sb, Expression expression)
        {
            if (expression == null)
                return;

            Type expressionType = ((expression is BinaryExpression) ? typeof(BinaryExpression) : expression.GetType());

            var exp = registeredExpressions.FirstOrDefault(p => p.Type == expressionType
                                                             || p.Type == expressionType.BaseType);
            if (exp != null)
            {
                exp.DecisionMaker = this;
                exp.Handle(sb, expression);
            }
            else
                throw new QueryBuilderException(String.Format("No Registered Expression! Type:{2} || NodeType:{0} || Expression:{1}",
                    expression.NodeType, //0
                    expression,  //1
                    expression.GetType())); //2
        }


    }
}
