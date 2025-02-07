using UnityEngine;

namespace Ui
{
    public abstract class UIContainerElement<DataType> : MonoBehaviour
    {
        public DataType Data { get; private set; }
        public void Setup(DataType data)
        {
            Data = data;
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