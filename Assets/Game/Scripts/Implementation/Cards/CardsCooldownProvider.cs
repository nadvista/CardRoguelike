using Core.Cards;
using Core.Tools.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implementation.Cards
{
    public class CardsCooldownProvider : ICardsCooldownProvider
    {
        private TimersPool _timersPool;
        private List<BaseCard> _blockedCards = new List<BaseCard>();
        private List<GameTimer> _timers = new List<GameTimer>();

        public event Action<BaseCard, GameTimer> OnBlockCard;
        public event Action<BaseCard, GameTimer> OnUnblockCard;

        public CardsCooldownProvider(TimersPool timersPool)
        {
            _timersPool = timersPool;
        }
        public bool IsCardBlocked(BaseCard card)
        {
            return _blockedCards.Contains(card);
        }

        public void BlockCard(BaseCard card)
        {
            var delay = card.CooldownSeconds;
            if (Mathf.Approximately(delay, 0))
                return;
            var timer = _timersPool.Get();

            _blockedCards.Add(card);
            _timers.Add(timer);
            timer.Start(delay, () =>
            {
                timer.Release();
                _timers.Remove(timer);
                _blockedCards.Remove(card);
                OnUnblockCard?.Invoke(card, timer);
            });

            OnBlockCard?.Invoke(card, timer);
        }

        public void Dispose()
        {
            Reset();
        }

        public void Reset()
        {
            foreach (var card in _blockedCards)
            {
                OnUnblockCard?.Invoke(card, null);
            }
            foreach (var timer in _timers)
            {
                timer.Release();
            }
            _blockedCards.Clear();
            _timers.Clear();
        }
    }
}
