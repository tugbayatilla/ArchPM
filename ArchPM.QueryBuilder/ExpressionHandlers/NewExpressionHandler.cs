using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    class NewExpressionHandler : IExpressionHandler
    {
        public DecisionMaker DecisionMaker { get; set; }
        public Type Type { get { return typeof(NewExpression); } }

        public void Handle(List<IContentItem> sb, Expression expression)
        {
            NewExpression newExp = expression as NewExpression;
            if (newExp == null)
                return;

            for (int i = 0; i < newExp.Arguments.Count; i++)
            {
                Expression exp = newExp.Arguments[i];
                MemberInfo memberInfo = newExp.Members[i];

                DecisionMaker.ExpressionHandle(sb, exp);
                FieldContentItem member = sb.FindLast(p => p is FieldContentItem) as FieldContentItem;
                if (memberInfo.Name != member.ValueStr)
                {
                    member.Alias = memberInfo.Name;
                }
            }
        }
    }
}
