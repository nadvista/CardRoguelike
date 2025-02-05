using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Ui
{
    public class UIContainer<ElementType, DataType> : MonoBehaviour where ElementType : UIContainerElement<DataType>
    {
        [SerializeField]
        private ElementType elementPrefab;

        [SerializeField]
        private Transform container;

        private List<ElementType> _elements = new List<ElementType>();
        private IInstantiator _instantiator;

        [Inject]
        private void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        private void Awake()
        {
            _elements = container.GetComponentsInChildren<ElementType>().ToList();
            foreach (var element in _elements)
                element.Deactivate();
        }
        public void Fill(List<DataType> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                ElementType element;
                if (_elements.Count == i)
                {
                    element = _instantiator.InstantiatePrefabForComponent<ElementType>(elementPrefab, container);
                    _elements.Add(element);
                }
                else
                {
                    element = _elements[i];
                }

                element.Setup(datas[i]);
                element.Activate();
            }
            for (int i = datas.Count; i < _elements.Count; i++)
            {
                var element = _elements[i];
                element.Deactivate();
            }
        }
    }
}