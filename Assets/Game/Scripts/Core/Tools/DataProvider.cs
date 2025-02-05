using System;
using System.Collections.Generic;

namespace Core.Tools
{
    public class DataProvider<T>
    {
        protected List<T> _availableData;

        public DataProvider(List<T> availableData, bool autoInitialize = true)
        {
            _availableData = availableData;
            if(autoInitialize)
                GetNew();
        }

        public T Current { get; private set; }

        public event Action<T> Changed;

        public T Get()
        {
            if(Current == null)
                return GetNew();
            return Current;
        }
        public T GetNew()
        {
            Current = _availableData[UnityEngine.Random.Range(0, _availableData.Count)];
            Changed?.Invoke(Current);
            return Current;
        }
    }
}
