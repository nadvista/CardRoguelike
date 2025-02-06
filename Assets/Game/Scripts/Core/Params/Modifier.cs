using Core.Tools;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Params
{
    public abstract class Modifier
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
