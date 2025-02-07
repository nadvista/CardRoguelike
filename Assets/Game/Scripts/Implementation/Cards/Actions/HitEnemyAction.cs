using Core.Actors;
using Core.Cards.Actions;
using Implementation.Params.Modifiers;
using System;
using UnityEngine;

namespace Implementation.Cards.Actions
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

        public override void DoAction(GameActor player, GameActor target, float timeBonus = 1)
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

            var subtract = ModifiersPool.Instance.GetSubtractModifier(int.MaxValue, scaledDamage);
            target.HealthParam.ApplyModifier(subtract);
        }
    }
}
