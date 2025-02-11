using Core.Cards;
using Core.ScoreCounting;
using Core.Tools.Timer;
using UnityEngine.UI;

namespace Ui.Cards
{
    public class CardCdView : CardUiComponent
    {
        private Image _viewFilledImage;

        private ICardsCooldownProvider _cooldownProvider;

        private GameTimer _cdTimer;

        public CardCdView(ICardsCooldownProvider cooldownProvider, Image viewFilledImage)
        {
            _cooldownProvider = cooldownProvider;
            _viewFilledImage = viewFilledImage;
        }
        public override void Start()
        {
            _cooldownProvider.OnBlockCard += OnBlockAnyCard;
            _cooldownProvider.OnUnblockCard += OnUnblockAnyCard;
        }
        public override void Dispose()
        {
            _cooldownProvider.OnBlockCard -= OnBlockAnyCard;
            _cooldownProvider.OnUnblockCard -= OnUnblockAnyCard;

            if (_cdTimer != null)
                _cdTimer.OnTick -= OnCdTimerTick;
        }

        private void OnBlockAnyCard(BaseCard card, GameTimer timer)
        {
            if (card != _card || _card == null)
                return;
            _cdTimer = timer;
            _cdTimer.OnTick += OnCdTimerTick;
        }

        private void OnCdTimerTick()
        {
            var fillAmount = (_card.CooldownSeconds - _cdTimer.CurrentTimeSeconds) / _card.CooldownSeconds;
            _viewFilledImage.fillAmount = fillAmount;
        }

        private void OnUnblockAnyCard(BaseCard card, GameTimer timer)
        {
            if (card != _card || _card == null)
                return;
            _viewFilledImage.fillAmount = 0;
            _cdTimer.OnTick -= OnCdTimerTick;
            _cdTimer = null;
        }
    }
}
