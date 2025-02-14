using Core.Actors;
using Core.Cards.Actions;
using Core.Params;
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
        private float ignoreDefenceScale;

        [SerializeField]
        private ParamType bonusAttackParamName;

        [SerializeField]
        private ParamType bonusDefenceParamName;

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
                    var defence = Mathf.Max(1, item.ActualValue - item.ActualValue * ignoreDefenceScale);
                    if (item.ActualValue != 0)
                        scaledDamage /= item.ActualValue;
                }
            }

            target.HealthParam.AddForeverValue(-scaledDamage);
        }
    }
}
