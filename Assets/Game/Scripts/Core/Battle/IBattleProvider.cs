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
        public event Action<GameActor, GameActor, CardsDesk> OnBattleStart;
        public event Action<BattleResult> OnBattleEnd;
        public event Action<PlayCardResult> OnPlayCard;

        public bool IsBattleStarted { get; }
        public GamePlayer CurrentPlayer { get; }
        public GameEnemy CurrentEnemy { get; }
        public CardsDesk CurrentCardsDesk { get; }

        public float TimeFromLastStep { get; }

        public Param PlayerMana { get; }
        public Param EnemyHeahth { get; }

        public void PlayBattleCard(BaseCard card);
        public void PlayBattleCard(int deskCardIndex);

        public void StopBattle();
        public void StartBattle();
    }
}
