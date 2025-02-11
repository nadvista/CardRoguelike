using Core.Actors;
using Core.Tools;
using Core.Tools.Timer;
using System;
using System.Collections.Generic;
using TMPro;

namespace Ui.Actors
{
    public class ActorHealthChangeVisualer : IDisposable
    {
        private const float SHOW_INDICATOR_TIME = 1f;
        private IEnumerable<TextMeshProUGUI> _hitIndicators;

        private TextMeshProUGUI _showingIndicator;

        private GameActor _actor;
        private float _lastHealthValue;

        private TimersPool _timersPool;
        private GameTimer _activeTimer;

        public ActorHealthChangeVisualer(IEnumerable<TextMeshProUGUI> hitIndicators, TimersPool timers)
        {
            _hitIndicators = hitIndicators;
            _timersPool = timers;
        }

        public void Setup(GameActor actor)
        {
            Reset();

            _actor = actor;
            _actor.HealthParam.OnValueChange += OnChangeHealth;
            _lastHealthValue = _actor.HealthParam.ActualValue;
            foreach(var indicator in _hitIndicators)
                indicator.gameObject.SetActive(false);
        }

        public void Dispose() 
        {
            UnsubscribeHealthChange();
        }
        public void Reset() 
        {
            UnsubscribeHealthChange();
            _activeTimer?.Release();
        }

        private void UnsubscribeHealthChange()
        {
            if (_actor != null)
                _actor.HealthParam.OnValueChange -= OnChangeHealth;
        }

        private void OnChangeHealth()
        {
            var difference = _lastHealthValue - _actor.HealthParam.ActualValue;
            _lastHealthValue = _actor.HealthParam.ActualValue;

            _showingIndicator?.gameObject.SetActive(false);

            _showingIndicator = _hitIndicators.RandomElement();
            _showingIndicator.text = $"-{difference.ToString()}";

            if (_activeTimer == null)
                _activeTimer = _timersPool.Get();

            _showingIndicator?.gameObject.SetActive(true);

            _activeTimer.Start(SHOW_INDICATOR_TIME, () => { 
                _showingIndicator.gameObject.SetActive(false );
                _activeTimer.Release();
            });
        }
    }
}
