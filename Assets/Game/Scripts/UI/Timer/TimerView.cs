using Core.Battle;
using Core.ScoreCounting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.Params
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField]
        private Image imageTimerFill;

        private IScoreCounter _scoreCounter;
        private IBattleStartController _battleStart;
        private IBattleStopController _battleStop;
        private IBattleCardsController _battleCards;

        private bool _isWorking;

        [Inject]
        private void Construct(IBattleStartController battleStart, IBattleStopController battleStop, IBattleCardsController battleCards, IScoreCounter scoreCounter)
        {
            _battleStart = battleStart;
            _battleStop = battleStop;
            _battleCards = battleCards;

            _scoreCounter = scoreCounter;
        }

        private void Start()
        {
            imageTimerFill.type = Image.Type.Filled;

            _battleStart.OnBattleStarted += OnBattleStart;
            _battleStop.OnBattleEnd += OnBattleEnd;
        }

        private void OnDestroy()
        {
            _battleStart.OnBattleStarted -= OnBattleStart;
            _battleStop.OnBattleEnd -= OnBattleEnd;
        }

        private void Update()
        {
            if (!_isWorking)
                return;

            var fillAmount = (_scoreCounter.BonusTimeSeconds - _battleCards.TimeFromLastCard) / _scoreCounter.BonusTimeSeconds;
            imageTimerFill.fillAmount = fillAmount;
        }

        private void OnBattleEnd(BattleResult result)
        {
            _isWorking = false;
            imageTimerFill.fillAmount = 1f;
        }
        private void OnBattleStart()
        {
            _isWorking = true;
        }
    }
}
