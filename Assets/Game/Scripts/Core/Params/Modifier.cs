using Core.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Params
{
    public abstract class Modifier : IDisposable
    {
        private static List<Modifier> _modifiers = new List<Modifier>();

        [SerializeField]
        public int Duration { get; private set; }

        public event Action<Modifier> OnModifierLate;

        public Modifier(int duration) 
        {
            Duration = duration;
            Initialize();
        }

        public void Dispose()
        {
            GlobalStepsManager.OnNewStep -= SubtractDuration;
        }

        private void Initialize()
        {
            _modifiers.Add(this);
            GlobalStepsManager.OnNewStep += SubtractDuration;
        }

        private void SubtractDuration()
        {
            if(--Duration <= 0)
            {
                GlobalStepsManager.OnNewStep -= SubtractDuration;
                OnModifierLate?.Invoke(this);
            }
        }

        public float Modify(float value)
        {
            if(Duration > 0)
                return GetModifiedValue(value);
            return value;
        }
        protected abstract float GetModifiedValue(float value);
    }
}
