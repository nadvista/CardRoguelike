using System;

namespace Core.Tools
{
    public static class GlobalStepsManager
    {
        public static event Action OnNewStep;

        public static void NewStep()
        {
            OnNewStep?.Invoke();
        }
    }
}
