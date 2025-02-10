using Core.Actors;
using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Cards;
using Core.Desk;
using Core.Params;
using System;

namespace Core.Battle
{
    public interface IBattleProvider
    {
        public event Action<GamePlayer, GameEnemy, CardsDesk> OnBattleStart;
        public event Action<BattleResult> OnBattleEnd;
        public event Action<BaseCard, PlayCardResult> OnPlayCard;
        public event Action<int, float> OnCardSwitchingStarted;

        public bool IsBattleStarted { get; }
        public GamePlayer CurrentPlayer { get; }
        public GameEnemy CurrentEnemy { get; }
        public CardsDesk CurrentCardsDesk { get; }

        public float TimeFromLastStepSeconds { get; }

        public Param PlayerMana { get; }
        public Param EnemyHeahth { get; }

        public void PlayBattleCard(int deskCardIndex);
        public void SwitchCardsPair(int number);

        public void StopBattle();
        public void StartBattle();
    }
}
