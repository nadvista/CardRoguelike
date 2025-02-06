using Core.Actors;
using System;

namespace Core.Cards.Actions
{
    [Serializable]
    public abstract class CardAction
    {
        public CardAction() { }
        public abstract void DoAction(GameActor player, GameActor target, float timeBonus = 1);
    }
}
