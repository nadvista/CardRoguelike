using Core.Actors;
using Core.Cards.Actions;
using Core.Params;
using System;
using System.Linq;
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
            var strength = player.GetParams(bonusAttackParamName).ToList();
            var defenseElemential = target.GetParams(bonusDefenceParamName).ToList();

            var defenceBase = target.GetParams(ParamType.Defence).ToList();

            var scaledDamage = damage * timeBonus;

            if (strength != null && strength.Count > 0)
            {
                foreach (var item in strength)
                {
                    if (item.ActualValue <= 1)
                        continue;
                    scaledDamage *= item.ActualValue;
                }
            }

            if (defenseElemential != null && defenseElemential.Count > 0)
            {
                foreach (var item in defenseElemential)
                {
                    if (item.Type == ParamType.Defence)
                        continue;
                    if (item.ActualValue <= 1)
                        continue;
                    scaledDamage /= item.ActualValue;
                }
            }

            if (defenceBase != null && defenceBase.Count > 0)
            {
                foreach (var item in defenceBase)
                {
                    if (item.ActualValue <= 1)
                        continue;
                    var defence = item.ActualValue - item.ActualValue * ignoreDefenceScale;
                    defence = Mathf.Max(defence, 1);
                    scaledDamage /= defence;
                }
            }

            target.HealthParam.AddForeverValue(-scaledDamage);
        }
    }
}
