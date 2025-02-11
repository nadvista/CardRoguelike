using Core.Battle;
using UnityEngine;
using Zenject;

namespace Implementation.Battle
{
    public class TestBattleInitializer : MonoBehaviour
    {
        private IBattleProvider _battle;

        [Inject]
        private void Construct(IBattleProvider battle)
        {
            _battle = battle;
        }

        private void Start()
        {
            _battle.PrepareBattle();
        }
    }
}
