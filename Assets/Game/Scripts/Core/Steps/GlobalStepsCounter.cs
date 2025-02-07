using System;

namespace Core.Steps
{
    public class GlobalStepsCounter : IStepCounter
    {
        public event Action OnNewStep;

        public void NewStep()
        {
            OnNewStep?.Invoke();
        }
    }
}
