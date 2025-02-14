using Core.Battle;
using Core.Cards;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Cards
{
    public class CardOnPlayView : CardUiComponent
    {
        private Image _imageIndicator;
        private IBattleProvider _battleProvider;

        private Sequence _animationSequence;

        private Color _stdColor;
        private Color _indicateColor;
        const float TOTAL_ANIMATION_TIME = 0.3f;

        public CardOnPlayView(IBattleProvider battleProvider, Image indicatorImage)
        {
            _imageIndicator = indicatorImage;
            _battleProvider = battleProvider;

            _stdColor = indicatorImage.color;
            _indicateColor = Color.red;
        }

        public override void Start()
        {
            _battleProvider.OnPlayCard += OnPlayAnyCard;
            _imageIndicator.gameObject.SetActive(false);
        }

        public override void Dispose()
        {
            _battleProvider.OnPlayCard -= OnPlayAnyCard;
            if (_animationSequence != null)
                _animationSequence.onComplete -= OnAnimationComplete;
        }

        private void OnPlayAnyCard(BaseCard card, PlayCardResult result)
        {
            if (card != _card || _card == null || result != PlayCardResult.Unsuccess)
                return;

            if (_animationSequence != null)
                return;

            _imageIndicator.gameObject.SetActive(true);
            _imageIndicator.color = _stdColor;

            _animationSequence = DOTween.Sequence();
            _animationSequence.Append(_imageIndicator.DOColor(_indicateColor, TOTAL_ANIMATION_TIME / 2));
            _animationSequence.Append(_imageIndicator.DOColor(_stdColor, TOTAL_ANIMATION_TIME / 2));

            _animationSequence.onComplete += OnAnimationComplete;
        }
        private void OnAnimationComplete()
        {
            _imageIndicator.gameObject.SetActive(false);
            _imageIndicator.color = _stdColor;
            _animationSequence = null;
        }
    }
}
