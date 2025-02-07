using Core.Battle;
using Core.Cards;
using Core.Tools.Timer;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Cards
{
    public class CardPlaySignal: IDisposable
    {
        private Image _signalImage;
        private float _totalAnimationTime;
        private int _animationSteps;

        private IBattleProvider _battle;
        private ICardsCooldownProvider _cooldownProvider;

        private BaseCard _card;

        private bool _isPlaying;
        private Sequence _sequence;
        private Color _transparentColor;
        private Color _standartColor;

        public CardPlaySignal(Image signalImage, 
            IBattleProvider battle, ICardsCooldownProvider cooldownProvider, BaseCard card)
        {
            _signalImage = signalImage;
            _totalAnimationTime = 0.6f;
            _animationSteps = 2;
            _battle = battle;
            _cooldownProvider = cooldownProvider;
            _card = card;

            _transparentColor = new Color(1, 1, 1, 0);
            _standartColor = signalImage.color;

            _signalImage.gameObject.SetActive(false);

            SubscribeEvents();
        }

        public void SetCard(BaseCard card)
        {
            _card = card;
        }

        public void Dispose()
        {
            UnsubscribeEvents();
            _sequence.Complete();
            _sequence.Kill();
        }

        private void SubscribeEvents() 
        {
            _battle.OnPlayCard += OnBattlePlayCard;
            _cooldownProvider.OnUnblockCard += OnUnblockCard;
        }
        private void UnsubscribeEvents() 
        {
            _battle.OnPlayCard -= OnBattlePlayCard;
            _cooldownProvider.OnUnblockCard -= OnUnblockCard;
        }

        private void OnUnblockCard(BaseCard card, GameTimer timer)
        {
            if (card != _card || !_isPlaying)
                return;

            _sequence?.Complete();
            _sequence?.Kill();
            _isPlaying = false;
        }

        private void OnBattlePlayCard(BaseCard card, PlayCardResult result)
        {
            if (card != _card || result != PlayCardResult.Unsuccess || _isPlaying)
                return;

            _signalImage.gameObject.SetActive(true);
            _signalImage.color = _transparentColor;
            _isPlaying = true;

            _sequence = DOTween.Sequence();
            var stepTime = _totalAnimationTime / (_animationSteps * 2);
            for(int i = 0; i < _animationSteps; i++)
            {
                _sequence.Append(_signalImage.DOColor(_standartColor, stepTime));
                _sequence.Append(_signalImage.DOColor(_transparentColor, stepTime));
            }

            _sequence.OnComplete(() =>
            {
                _signalImage.color = _standartColor;
                _signalImage.gameObject.SetActive(false);
                _isPlaying = false;
            });
        }
    }
}
