using Core.Cards;
using Core.Desk;
using System;

namespace Core.Battle
{
    public interface IBattleCardsController
    {
        public event Action<BaseCard, PlayCardResult> OnPlayCard;
        public event Action<int, float> OnCardSwitchingStarted;

        public CardsDesk CurrentCardsDesk { get; }
        public float TimeFromLastCard { get; }

        public void PlayBattleCard(int deskCardIndex);
        public void SwitchCardsPair(int number);
    }
}
