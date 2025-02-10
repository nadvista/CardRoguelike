using System;
using UnityEngine;

namespace Core.ScoreCounting
{
    [Serializable]
    public class ScoreCounter : IScoreCounter
    {
        [field: SerializeField]
        public float BonusTimeSeconds { get; private set; }

        [field: SerializeField]
        public float BonusScale { get; private set; }

        public float CalculateMaxBonus()
        {
            return 1 + BonusScale;
        }

        public float CalculateScore(float time)
        {
            if(BonusTimeSeconds == 0)
            {
                Debug.LogError("Не задано бонусное время");
                BonusTimeSeconds = 0.1f;
            }
            var result = 1 + (1 - Mathf.Min(time, BonusTimeSeconds) / BonusTimeSeconds) * BonusScale;
            return result;
        }
    }
}
