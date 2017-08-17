using System;

namespace ArchPM.Core
{
    public interface IComboData<T, U>
    {
        T key { get; set; }
        U value { get; set; }
    }

}
