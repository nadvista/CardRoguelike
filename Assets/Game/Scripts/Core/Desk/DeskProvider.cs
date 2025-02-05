using Core.Tools;
using NUnit.Framework;
using System;
using System.Security.Cryptography;

namespace Core.Desk
{
    public class DeskProvider
    {
        private DataProvider<CardsDesk> _provider;

        public CardsDesk CurrentDesk => _provider.Current;

        public event Action<CardsDesk> OnDeskChanged;
        public DeskProvider(DesksDatabase desks)
        {
            _provider = new DataProvider<CardsDesk>(desks.Desks, false);
            _provider.Changed += DeskChanged;
            SelectNewDesk();
        }

        public void SelectNewDesk()
        {
            _provider.GetNew();
        }
        private void DeskChanged(CardsDesk desk)
        {
            OnDeskChanged?.Invoke(desk);
        }
    }
}
