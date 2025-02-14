using Core.Params;
using Core.Steps;
using UnityEngine;

namespace Implementation.Params.Modifiers
{
    public class MultiplyModifier : Modifier
    {
        [SerializeField]
        private float scaleValue;

        public MultiplyModifier(IStepCounter stepsCounter) : base(stepsCounter)
        {
        }

        public void SetupValue(float value)
        {
            scaleValue = value;
        }

        protected override float GetModifiedValue(float value)
        {
            return value * scaleValue;
        }
    }
}
