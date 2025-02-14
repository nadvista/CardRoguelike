using Core.Steps;
using Core.Tools.Pool;

namespace Implementation.Params.Modifiers
{
    public class ModifierFabric : IPoolFabric<SubtractModifier>, IPoolFabric<MultiplyModifier>
    {
        private IStepCounter _stepCounter;

        public ModifierFabric(IStepCounter stepCounter)
        {
            _stepCounter = stepCounter;
        }

        public SubtractModifier CreateNew()
        {
            return new SubtractModifier(_stepCounter);
        }

        MultiplyModifier IPoolFabric<MultiplyModifier>.CreateNew()
        {
            return new MultiplyModifier(_stepCounter);
        }
    }
}
