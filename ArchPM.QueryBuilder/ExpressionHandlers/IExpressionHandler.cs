using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ArchPM.QueryBuilder.ExpressionHandlers
{
    public interface IExpressionHandler
    {
        Type Type { get; }
        DecisionMaker DecisionMaker { get; set; }
        void Handle(List<IContentItem> sb, Expression expression);
    }
}
