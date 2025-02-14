using Core.Actors;
using Core.Cards.Actions;
using Core.Params;
using Implementation.Params.Modifiers;
using System;
using UnityEngine;

namespace Implementation.Cards.Actions
{
    [Serializable]
    public class DecreaseParamAction : CardAction
    {
        [SerializeField]
        private float decreaseScale;

        [SerializeField]
        private int duration;

        [SerializeField]
        private ParamType bonusAttackParamName;

        [SerializeField]
        private ParamType decreasingParam;

        public override void DoAction(GameActor player, GameActor target, float timeBonus = 1)
        {
            var strength = player.GetParams(bonusAttackParamName);
            var decreasing = target.GetParams(decreasingParam)[0];
            var scaledDamage = decreaseScale;

            if (strength != null && strength.Count > 0)
            {
                foreach (var item in strength)
                {
                    scaledDamage *= item.ActualValue;
                }
            }

            var modifier = ModifiersPool.Instance.GetMultiplyModifier(duration, scaledDamage);
            decreasing.ApplyModifier(modifier);
        }
    }
}
