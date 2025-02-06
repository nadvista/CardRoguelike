using Core.Params;
using UnityEngine;

namespace Implementation.Params.Modifiers
{
    public class SubtractModifier : Modifier
    {
        [SerializeField]
        private float subtractValue;

        public SubtractModifier(float value, int duration) : base(duration)
        {
            subtractValue = value;
        }

        protected override float GetModifiedValue(float value)
        {
            return value - subtractValue;
        }
    }
}
