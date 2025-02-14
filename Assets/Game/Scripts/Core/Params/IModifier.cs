using System;

namespace Core.Params
{
    public interface IModifier
    {
        public int Duration { get; }
        public event Action<Modifier> OnModifierLate;

        public float Modify(float value);
        public void SetupDuration(int value);
    }
}
