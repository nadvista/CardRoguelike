using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Params
{
    [Serializable]
    public class Param : IDisposable
    {
        [field: SerializeField]
        public ParamType Type { get; private set; }

        [SerializeField]
        private float baseValue;

        [SerializeField]
        private float minValue;

        [SerializeField]
        private float maxValue;

        private float _foreverAdditionValue = 0;

        public float Max => maxValue;
        public float Min => minValue;

        public Param(float baseValue, float minValue, float maxValue)
        {
            this.baseValue = baseValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public List<Modifier> Modifiers
        {
            get
            {
                if (_modifiers == null)
                    _modifiers = new List<Modifier>();
                return _modifiers;
            }
        }
        private List<Modifier> _modifiers;

        public event Action OnValueChange;

        public float ActualValue
        {
            get
            {
                var result = baseValue;
                var toRemoveMods = new List<Modifier>();
                foreach (var modifier in Modifiers)
                {
                    if (modifier.Duration <= 0)
                        toRemoveMods.Add(modifier);
                    else
                        result = modifier.Modify(result);
                }
                Modifiers.RemoveAll(e => toRemoveMods.Contains(e));
                result += _foreverAdditionValue;
                return Mathf.Clamp(result, minValue, maxValue);
            }
        }
        public void AddForeverValue(float value)
        {
            _foreverAdditionValue += value;
            OnValueChange?.Invoke();
        }
        public void ApplyModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
            OnValueChange?.Invoke();
            modifier.OnModifierLate += OnModifierLate;
        }
        public void RemoveModifier(Modifier modifier)
        {
            Modifiers.Remove(modifier);
            modifier.OnModifierLate -= OnModifierLate;
            OnValueChange?.Invoke();
        }
        public void Reset()
        {
            RemoveEventsListeners();
            _foreverAdditionValue = 0f;
            Modifiers?.Clear();
            OnValueChange?.Invoke();
        }

        public void Dispose()
        {
            RemoveEventsListeners();
        }

        private void RemoveEventsListeners()
        {
            foreach (var modifier in Modifiers)
                modifier.OnModifierLate -= OnModifierLate;
        }
        private void OnModifierLate(Modifier modifier)
        {
            RemoveModifier(modifier);
        }
    }
}
