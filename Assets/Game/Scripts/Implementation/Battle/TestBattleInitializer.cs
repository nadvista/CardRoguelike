using Core.Battle;
using UnityEngine;
using Zenject;

namespace Implementation.Battle
{
    public class TestBattleInitializer : MonoBehaviour
    {
        private IBattlePrepareController _battle;

        [Inject]
        private void Construct(IBattlePrepareController battlePrepare)
        {
            _battle = battlePrepare;
        }

        private void Start()
        {
            _battle.PrepareBattle();
        }
    }
}
