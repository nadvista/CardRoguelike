using Core.Actors;
using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Battle;
using Core.Desk;
using Core.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Ui.Actors
{
    public class ActorHealthChangeVisualer : MonoBehaviour
    {
        [SerializeField]
        private ActorViewType type;

        [SerializeField]
        private float showIndicatorTimeSeconds = 1f;

        [SerializeField]
        private List<TextMeshProUGUI> hitIndicators;

        private TextMeshProUGUI _showingIndicator;

        private GameActor _actor;
        private float _lastHealthValue;

        private Coroutine _showIndicatorCoroutine;

        private IBattlePrepareController _battlePrepare;
        private IBattleStopController _battleStop;

        [Inject]
        private void Construct(IBattlePrepareController battlePrepare, IBattleStopController battleStop)
        {
            _battlePrepare = battlePrepare;
            _battleStop = battleStop;
        }

        private void Awake()
        {
            _battlePrepare.OnBattlePrepared += OnBattlePrepare;
            _battleStop.OnBattleEnd += OnBattleEnd;
        }
        private void Start()
        {
            foreach (var indicator in hitIndicators)
                indicator.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _battlePrepare.OnBattlePrepared -= OnBattlePrepare;
            _battleStop.OnBattleEnd -= OnBattleEnd;

            ResetActor();
        }

        private void OnBattleEnd(BattleResult result)
        {
            ResetActor();
        }

        private void OnBattlePrepare(GamePlayer player, GameEnemy enemy, CardsDesk desk)
        {
            switch (type)
            {
                case ActorViewType.Player:
                    Setup(player);
                    return;
                case ActorViewType.Enemy:
                    Setup(enemy);
                    return;
            }
        }

        private void Setup(GameActor actor)
        {
            ResetActor();
            _actor = actor;
            _lastHealthValue = _actor.HealthParam.ActualValue;
            _actor.HealthParam.OnValueChange += OnHealthChange;
        }

        private void ResetActor()
        {
            if (_actor != null)
                _actor.HealthParam.OnValueChange -= OnHealthChange;
            StopHideCoroutine();
            _showingIndicator?.gameObject.SetActive(false);
        }

        private void StopHideCoroutine()
        {
            if (_showIndicatorCoroutine != null)
                StopCoroutine(_showIndicatorCoroutine);
        }

        private void OnHealthChange()
        {
            var diff = _actor.HealthParam.ActualValue - _lastHealthValue;
            _lastHealthValue = _actor.HealthParam.ActualValue;

            _showingIndicator?.gameObject.SetActive(false);

            _showingIndicator = hitIndicators.RandomElement();
            _showingIndicator.text = diff.ToString("F2");
            _showingIndicator.gameObject.SetActive(true);

            StopHideCoroutine();
            _showIndicatorCoroutine = StartCoroutine(HideIndicatorCoroutine());
        }

        private IEnumerator HideIndicatorCoroutine()
        {
            yield return new WaitForSeconds(showIndicatorTimeSeconds);
            _showingIndicator.gameObject.SetActive(false);
            _showIndicatorCoroutine = null;
        }
    }
}
