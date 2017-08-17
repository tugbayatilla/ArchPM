using System;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using ArchPM.Core.Extensions;
using System.Collections;
using ArchPM.Core.Extensions.ObjectExtensions;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    class ConstantExpressionHandler : IExpressionHandler
    {
        public DecisionMaker DecisionMaker { get; set; }
        public Type Type { get { return typeof(ConstantExpression); } }

        public void Handle(List<IContentItem> sb, Expression expression)
        {
            ConstantExpression constantExp = expression as ConstantExpression;
            if (constantExp == null)
                return;

            sb.Add(new ValueContentItem() { Value = constantExp.Value });
        }
    }
}
