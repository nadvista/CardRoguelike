using System.Collections.Generic;

namespace Core.Tools.Pool
{
    public class ObjectsPool<PoolElementType> where PoolElementType : IPoolElement
    {
        protected List<PoolElementType> _allElements { get; private set; } = new List<PoolElementType>();
        protected List<PoolElementType> _workingElements { get; private set; } = new List<PoolElementType>();
        protected Stack<PoolElementType> _inactiveElements { get; private set; } = new Stack<PoolElementType>();

        protected List<PoolElementType> _toremove { get; private set; } = new List<PoolElementType>();

        protected IPoolFabric<PoolElementType> _fabric { get; private set; }

        public ObjectsPool(IPoolFabric<PoolElementType> fabric)
        {
            _fabric = fabric;
        }
        public PoolElementType Get()
        {
            if(_inactiveElements.Count == 0)
            {
                CollectInactiveElements();
            }
            if (_inactiveElements.Count == 0)
            {
                CreateNew();
            }

            var element = UseInactiveElement();
            element.OnTakeFromPool();

            return element;
        }

        private PoolElementType CreateNew()
        {
            var element = _fabric.CreateNew();
            _allElements.Add(element);
            _inactiveElements.Push(element);
            element.OnReturnToPool();
            return element;
        }

        private PoolElementType UseInactiveElement()
        {
            var element = _inactiveElements.Pop();
            _workingElements.Add(element);
            return element;
        }

        private void CollectInactiveElements()
        {
            foreach(var element in _workingElements)
            {
                if(element.IsInactive)
                    _toremove.Add(element);
            }
            foreach(var element in _toremove)
            {
                element.OnReturnToPool();
                _workingElements.Remove(element);
                _inactiveElements.Push(element);
            }
            _toremove.Clear();
        }
    }
}
