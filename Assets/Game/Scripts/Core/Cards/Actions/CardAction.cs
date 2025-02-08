using Core.Actors;
using Core.Tools;
using System;

namespace Core.Cards.Actions
{
    [Serializable]
    public abstract class CardAction : IAbstractSetup
    {
        public CardAction() { }
        public abstract void DoAction(GameActor player, GameActor target, float timeBonus = 1);
    }
}
