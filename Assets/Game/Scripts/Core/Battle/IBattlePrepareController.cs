using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Desk;
using System;

namespace Core.Battle
{
    public interface IBattlePrepareController
    {
        public event Action<GamePlayer, GameEnemy, CardsDesk> OnBattlePrepared;

        public bool IsBattlePrepared { get; }

        public void PrepareBattle();
    }
}
