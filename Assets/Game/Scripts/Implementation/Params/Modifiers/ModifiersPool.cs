using Core.Steps;
using Core.Tools.Pool;

namespace Implementation.Params.Modifiers
{
    public class ModifiersPool
    {
        public static ModifiersPool Instance { get; private set; }

        private ObjectsPool<SubtractModifier> _subtractModifiersPool;

        public ModifiersPool(IPoolFabric<SubtractModifier> subtractModFabric)
        {
            _subtractModifiersPool = new ObjectsPool<SubtractModifier>(subtractModFabric);
            if (Instance != null)
            {
                UnityEngine.Debug.Log($"Singleton {nameof(ModifiersPool)} was already initialized");
            }
            else
            {
                Instance = this;
            }
        }

        public SubtractModifier GetSubtractModifier(int duration, float value)
        {
            var result = _subtractModifiersPool.Get();
            result.SetupDuration(duration);
            result.SetupValue(value);
            return result;
        }
    }
}
