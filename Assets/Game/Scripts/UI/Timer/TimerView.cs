using Core.Actors;
using Core.Battle;
using Core.Desk;
using Core.ScoreCounting;
using Core.Steps;
using System;
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
        private IBattleProvider _battleProvider;

        private bool _isWorking;

        [Inject]
        private void Construct(IBattleProvider battleProvider, IScoreCounter scoreCounter)
        {
            _battleProvider = battleProvider;
            _scoreCounter = scoreCounter;
        }

        private void Start()
        {
            imageTimerFill.type = Image.Type.Filled;

            _battleProvider.OnBattleStart += OnBattleStart;
            _battleProvider.OnBattleEnd += OnBattleEnd;
        }

        private void OnDestroy()
        {
            _battleProvider.OnBattleStart -= OnBattleStart;
            _battleProvider.OnBattleEnd -= OnBattleEnd;
        }

        private void Update()
        {
            if (!_isWorking)
                return;

            var fillAmount = (_scoreCounter.BonusTimeSeconds - _battleProvider.TimeFromLastStepSeconds) / _scoreCounter.BonusTimeSeconds;
            imageTimerFill.fillAmount = fillAmount;
        }

        private void OnBattleEnd(BattleResult result)
        {
            _isWorking = false;
        }
        private void OnBattleStart(GameActor actor1, GameActor actor2, CardsDesk desk)
        {
            _isWorking = true;
        }
    }
}
