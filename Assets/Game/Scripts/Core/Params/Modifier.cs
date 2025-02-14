using Core.Steps;
using Core.Tools.Pool;
using System;
using UnityEngine;

namespace Core.Params
{
    public abstract class Modifier : IModifier, IDisposable, IPoolElement
    {
        [SerializeField]
        public int Duration { get; private set; }

        private IStepCounter _stepsCounter;

        protected Modifier(IStepCounter stepsCounter)
        {
            _stepsCounter = stepsCounter;
        }

        public bool IsInactive => Duration <= 0;

        public event Action<Modifier> OnModifierLate;

        public float Modify(float value)
        {
            if (Duration > 0)
                return GetModifiedValue(value);
            return value;
        }

        public void OnReturnToPool()
        {
            _stepsCounter.OnNewStep -= OnNewStep;
        }

        public void OnTakeFromPool()
        {
            _stepsCounter.OnNewStep += OnNewStep;
        }

        public void SetupDuration(int duration)
        {
            Duration = duration;
        }
        public void Dispose()
        {
            _stepsCounter.OnNewStep -= OnNewStep;
        }

        private void OnNewStep()
        {
            Duration--;
            if (Duration <= 0)
            {
                OnModifierLate?.Invoke(this);
                _stepsCounter.OnNewStep -= OnNewStep;
            }
        }

        protected abstract float GetModifiedValue(float value);
    }
}
