using ArchPM.QueryBuilder.ContentItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.QueryBuilder.MethodCalls
{
    public interface IMethodCall
    {
        String Name { get; }

        void Handle(DecisionMaker DecisionMaker, List<IContentItem> sb, MethodCallExpression expression);
    }
}
