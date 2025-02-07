using Core.Steps;
using Core.Tools.Pool;

namespace Implementation.Params.Modifiers
{
    public class SubtractModifierFabric : IPoolFabric<SubtractModifier>
    {
        private IStepCounter _stepCounter;

        public SubtractModifierFabric(IStepCounter stepCounter)
        {
            _stepCounter = stepCounter;
        }

        public SubtractModifier CreateNew()
        {
            return new SubtractModifier(_stepCounter);
        }
    }
}
