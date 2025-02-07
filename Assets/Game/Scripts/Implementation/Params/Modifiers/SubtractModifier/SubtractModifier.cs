using Core.Params;
using Core.Steps;
using UnityEngine;

namespace Implementation.Params.Modifiers
{
    public class SubtractModifier : Modifier
    {
        [SerializeField]
        private float subtractValue;

        public SubtractModifier(IStepCounter stepCounter) : base(stepCounter)
        {
        }

        public void SetupValue(float value)
        {
            subtractValue = value;
        }

        protected override float GetModifiedValue(float value)
        {
            return value - subtractValue;
        }
    }
}
