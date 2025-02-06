using Core.Actors;
using Core.Params;
using System;
using UnityEngine;

namespace Core.Cards.Actions
{
    [Serializable]
    public class HitEnemyAction : CardAction
    {
        [SerializeField]
        private float damage;

        [SerializeField]
        private string bonusAttackParamName;

        [SerializeField]
        private string bonusDefenceParamName;

        public override void DoAction(Actor player, Actor target, float timeBonus = 1)
        {
            var strength = player.GetParams(bonusAttackParamName);
            var defense = target.GetParams(bonusDefenceParamName);
            var scaledDamage = damage * timeBonus;

            if (strength != null && strength.Count > 0)
            {
                foreach (var item in strength)
                {
                    scaledDamage *= item.ActualValue;
                }
            }

            if (defense != null && defense.Count > 0)
            {
                foreach (var item in defense)
                {
                    scaledDamage /= item.ActualValue;
                }
            }

            target.HealthParam.ApplyModifier(new SubtractModifier(scaledDamage, int.MaxValue));
            Debug.Log($"Hit {scaledDamage}");
        }
    }
}
