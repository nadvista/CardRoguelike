using Core.Tools.Pool;

namespace Implementation.Params.Modifiers
{
    public class ModifiersPool
    {
        public static ModifiersPool Instance { get; private set; }

        private ObjectsPool<SubtractModifier> _subtractModifiersPool;
        private ObjectsPool<MultiplyModifier> _multiplyModifiersPool;

        public ModifiersPool(IPoolFabric<SubtractModifier> subtractModFabric, IPoolFabric<MultiplyModifier> multModFabric)
        {
            _subtractModifiersPool = new ObjectsPool<SubtractModifier>(subtractModFabric);
            _multiplyModifiersPool = new ObjectsPool<MultiplyModifier>(multModFabric);
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

        public MultiplyModifier GetMultiplyModifier(int duration, float scale)
        {
            var result = _multiplyModifiersPool.Get();
            result.SetupDuration(duration);
            result.SetupValue(scale);
            return result;
        }
    }
}
