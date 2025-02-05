using System;
using UnityEngine;

namespace Core.Tools
{
    [Serializable]
    public class ScoreCounter
    {
        [field: SerializeField]
        public float BonusTime { get; private set; }

        [field: SerializeField]
        public float BonusScale { get; private set; }

        public virtual float CalculateMaxBonus()
        {
            return 1 + BonusScale;
        }

        public virtual float CalculateScore(float time)
        {
            var result = 1 + (1 - Mathf.Min(time, BonusTime) / BonusTime) * BonusScale;
            return result;
        }
    }
}
