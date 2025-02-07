using Core.Cards;
using Core.Tools.Timer;
using System;
using UnityEngine.UI;

namespace Ui.Cards
{
    public class CardCooldownSignal : IDisposable
    {
        private Image _blockImage;
        private ICardsCooldownProvider _cooldownProvider;
        private BaseCard _card;
        private GameTimer _blockTimer;

        public CardCooldownSignal(BaseCard card, Image blockImage, ICardsCooldownProvider cooldownProvider)
        {
            _blockImage = blockImage;
            _cooldownProvider = cooldownProvider;
            _card = card;

            _cooldownProvider.OnBlockCard += OnBlockCard;
            _cooldownProvider.OnUnblockCard += OnUnblockCard;

            _blockImage.fillAmount = 0;
        }

        public void SetCard(BaseCard card)
        {
            _card = card;
        }

        private void OnBlockCard(BaseCard card, GameTimer timer)
        {
            if (card != _card || _card == null)
                return;
            _blockTimer = timer;
            _blockTimer.OnTick += OnTimerTick;
            _blockImage.fillAmount = 1;
        }

        private void OnTimerTick()
        {
            if (_card == null || _blockTimer == null)
                return;
            var fillAmount = (_card.PlayDelay - _blockTimer.CurrentTimeSeconds) / _card.PlayDelay;
            _blockImage.fillAmount = fillAmount;
        }

        private void OnUnblockCard(BaseCard card, GameTimer timer)
        {
            if (card != _card || _card == null)
                return;
            _blockImage.fillAmount = 0;
            _blockTimer.OnTick -= OnTimerTick;
            _blockTimer = null;
        }

        public void Dispose()
        {
            if (_blockTimer != null)
                _blockTimer.OnTick -= OnTimerTick;

            _cooldownProvider.OnBlockCard -= OnBlockCard;
            _cooldownProvider.OnUnblockCard -= OnUnblockCard;
        }
    }
}
