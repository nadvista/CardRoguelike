using System;

namespace Core.Inputs
{
    public interface IInputsHandler : IDisposable
    {
        public event Action<int> OnPlayPerformed;
        public event Action<int> OnSwitchPerformed;
    }
}