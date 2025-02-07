using System.Collections.Generic;

namespace Core.Tools.Pool
{
    public class ObjectsPool<PoolElementType> where PoolElementType : IPoolElement
    {
        private List<PoolElementType> _allElements = new List<PoolElementType>();
        private List<PoolElementType> _workingElements = new List<PoolElementType>();
        private Stack<PoolElementType> _inactiveElements = new Stack<PoolElementType>();

        private List<PoolElementType> _toremove = new List<PoolElementType>();

        private IPoolFabric<PoolElementType> _fabric;

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
