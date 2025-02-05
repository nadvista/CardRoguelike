using UnityEngine;

namespace Ui
{
    public abstract class UIContainerElement<DataType> : MonoBehaviour
    {
        protected DataType _data { get; private set; }
        public void Setup(DataType data)
        {
            _data = data;
            OnSetup(data);
        }
        public void Deactivate()
        {
            OnDeactivate();
            gameObject.SetActive(false);
        }
        public void Activate()
        {
            OnActivate();
            gameObject.SetActive(true);
        }

        protected abstract void OnSetup(DataType data);

        protected virtual void OnDeactivate() { }
        protected virtual void OnActivate() { }
    }
}