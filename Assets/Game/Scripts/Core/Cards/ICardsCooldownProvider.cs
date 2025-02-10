using Core.Tools.Timer;
using System;

namespace Core.Cards
{
    public interface ICardsCooldownProvider : IDisposable
    {
        public bool IsCardBlocked(BaseCard card);
        public void BlockCard(BaseCard card);
        public void Reset();
        public event Action<BaseCard, GameTimer> OnBlockCard;
        public event Action<BaseCard, GameTimer> OnUnblockCard;
    }
}
