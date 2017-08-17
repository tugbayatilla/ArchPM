using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using ArchPM.Core.Extensions.ObjectExtensions;
using ArchPM.Core.Extensions.TypeExtensions;
using ArchPM.Core.Extensions;
using ArchPM.QueryBuilder.ContentItems;

namespace ArchPM.QueryBuilder.MethodCalls
{
    class Search : IMethodCall
    {
        public String Name { get { return "Search"; } }

        public void Handle(DecisionMaker DecisionMaker, List<IContentItem> sb, MethodCallExpression expression)
        {
            foreach (var obj in expression.Arguments)
            {
                DecisionMaker.ExpressionHandle(sb, obj);
            }
        }
    }
}
