using System;

namespace Core.Battle
{
    public interface IBattleStopController
    {
        public event Action<BattleResult> OnBattleEnd;

        public void StopBattle();
    }
}
