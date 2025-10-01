using System;

namespace Core.Battle
{
    public interface IBattleStartController
    {
        public event Action OnBattleStarted;
        public bool IsBattleStarted { get; }

        public void StartBattle();
    }
}
