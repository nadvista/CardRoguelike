using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Params
{
    [Serializable]
    public class Param
    {
        [field: SerializeField]
        public string ParamName { get; private set; }

        [SerializeField]
        private float baseValue;

        [SerializeField]
        private float minValue;

        [SerializeField]
        private float maxValue;

        public Param(float baseValue, float minValue, float maxValue)
        {
            this.baseValue = baseValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public List<Modifier> Modifiers { 
            get 
            { 
                if(_modifiers == null)
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
                    result = modifier.Modify(result);
                    if(modifier.Duration <= 0)
                        toRemoveMods.Add(modifier);
                }
                Modifiers.RemoveAll(e => toRemoveMods.Contains(e));
                return Mathf.Clamp(result, minValue, maxValue);
            }
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
        private void OnModifierLate(Modifier modifier)
        {
            RemoveModifier(modifier);
        }

        public void Reset()
        {
            Modifiers?.Clear();
            OnValueChange?.Invoke();
        }
    }
}
