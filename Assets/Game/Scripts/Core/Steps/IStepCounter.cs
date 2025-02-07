using System;

namespace Core.Steps
{
    public interface IStepCounter
    {
        public event Action OnNewStep;
    }
}
