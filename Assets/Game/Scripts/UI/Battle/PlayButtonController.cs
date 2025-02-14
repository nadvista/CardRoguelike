using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Battle;
using Core.Desk;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PlayButtonController : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        private IBattleProvider _battle;

        [Inject]
        private void Construct(IBattleProvider battle)
        {
            _battle = battle;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
            button.onClick.AddListener(OnButtonClicked);
            _battle.OnBattlePrepared += OnBattlePrepare;
            _battle.OnBattleEnd += OnBattleEnd;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);
            _battle.OnBattlePrepared -= OnBattlePrepare;
            _battle.OnBattleEnd -= OnBattleEnd;
        }

        private void OnButtonClicked()
        {
            if (_battle.IsBattleStarted)
                return;

            if (!_battle.IsBattlePrepared)
            {
                _battle.PrepareBattle();
                return;
            }

            _battle.StartBattle();
            gameObject.SetActive(false);
        }

        private void OnBattlePrepare(GamePlayer player, GameEnemy enemy, CardsDesk desk)
        {
            gameObject.SetActive(true);
        }

        private void OnBattleEnd(BattleResult result)
        {
            gameObject.SetActive(true);
        }
    }
}