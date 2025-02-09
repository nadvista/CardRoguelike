using Core.Cards;
using System;

namespace Ui.Cards
{
    public abstract class CardUiComponent : IDisposable
    {
        protected BaseCard _card { get; private set; }

        public virtual void Start() { }
        public void SetCard(BaseCard card)
        {
            _card = card;
        }

        public abstract void Dispose();
    }
}
