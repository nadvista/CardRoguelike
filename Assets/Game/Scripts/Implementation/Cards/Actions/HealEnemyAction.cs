using Core.Actors;
using Core.Cards.Actions;
using System;
using UnityEngine;

namespace Implementation.Cards.Actions
{
    [Serializable]
    public class HealEnemyAction : CardAction
    {
        [SerializeField]
        private float healPoints;

        public override void DoAction(GameActor player, GameActor target, float timeBonus = 1)
        {
            var maxAmount = target.HealthParam.Max - target.HealthParam.ActualValue;
            var amount = Mathf.Min(maxAmount, healPoints);
            
            target.HealthParam.AddForeverValue(amount);
        }
    }
}
