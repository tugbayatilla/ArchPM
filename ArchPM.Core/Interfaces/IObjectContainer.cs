using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchPM.Core
{
    public interface IObjectContainer
    {
        T Resolve<T>(String name = "");
        IEnumerable<T > ResolveAll<T>();
    }
}
