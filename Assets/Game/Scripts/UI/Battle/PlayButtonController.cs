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

        private IBattleStartController _battleStart;
        private IBattlePrepareController _battlePrepare;
        private IBattleStopController _battleStop;

        [Inject]
        private void Construct(IBattleStartController battleStart, IBattlePrepareController battlePrepare, IBattleStopController battleStop)
        {
            _battleStart = battleStart;
            _battlePrepare = battlePrepare;
            _battleStop = battleStop;
        }

        private void Awake()
        {
            gameObject.SetActive(false);

            button.onClick.AddListener(OnButtonClicked);

            _battlePrepare.OnBattlePrepared += OnBattlePrepare;
            _battleStop.OnBattleEnd += OnBattleEnd;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);

            _battlePrepare.OnBattlePrepared -= OnBattlePrepare;
            _battleStop.OnBattleEnd -= OnBattleEnd;
        }

        private void OnButtonClicked()
        {
            if (_battleStart.IsBattleStarted)
                return;

            if (!_battlePrepare.IsBattlePrepared)
            {
                _battlePrepare.PrepareBattle();
                return;
            }

            _battleStart.StartBattle();
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