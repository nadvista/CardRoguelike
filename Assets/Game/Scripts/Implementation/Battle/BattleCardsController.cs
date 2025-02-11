using Codice.Client.Common;
using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Battle;
using Core.Cards;
using Core.Desk;
using Core.Tools.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Implementation.Battle
{
    internal partial class BattleCardsController : IDisposable
    {
        private IBattleProvider _battleProvider;
        private ICardsCooldownProvider _cardsCooldownProvider;
        private CardsDesk _currentDesk;
        private TimersPool _timersPool;
        private List<Pair> _pairs = new List<Pair>();

        public BattleCardsController(ICardsCooldownProvider cardsCooldownProvider, IBattleProvider battle, TimersPool timers)
        {
            _cardsCooldownProvider = cardsCooldownProvider;
            _timersPool = timers;
            _battleProvider = battle;
        }

        public void SetupDesk(CardsDesk desk)
        {
            _currentDesk = desk;
            Reset();

            _pairs.Clear();

            var pairsCount = Mathf.CeilToInt(desk.Cards.Count / 2);

            for (int pair = 0; pair < pairsCount; pair++)
            {
                var card1Index = pair * 2;
                var card2Index = pair * 2 + 1;

                var card1 = desk.Cards[card1Index];
                var card2 = desk.Cards.Count > card2Index ? desk.Cards[card2Index] : card1;

                var pairElement = new Pair(card1, card2, _timersPool.Get());
                _pairs.Add(pairElement);
            }
        }
        public void Dispose()
        {
            foreach (var pair in _pairs)
                pair.ReleaseTimer();
            _cardsCooldownProvider.Dispose();
        }
        public void Reset()
        {
            foreach(var pair in _pairs)
                pair.ReleaseTimer(); 
            _cardsCooldownProvider.Reset();
        }

        public PlayCardResult PlayBattleCard(int cardIndex, GamePlayer player, GameEnemy enemy, float timeBonus)
        {
            if (cardIndex >= _currentDesk.Cards.Count)
                return PlayCardResult.Unsuccess;

            var card = GetCardByIndex(cardIndex);

            return PlayBattleCard(card, player, enemy, timeBonus);
        }
        private PlayCardResult PlayBattleCard(BaseCard card, GamePlayer player, GameEnemy enemy, float timeBonus)
        {
            if (!CanPlayCard(card, player))
                return PlayCardResult.Unsuccess;

            player.HealthParam.AddForeverValue(-card.ManaCost.ActualValue);

            foreach (var action in card.Actions)
                action.DoAction(player, enemy, timeBonus);

            _cardsCooldownProvider.BlockCard(card);

            return PlayCardResult.Success;
        }

        public float SwitchCards(int switchIndex, float totalTime)
        {
            if (switchIndex >= _pairs.Count)
                return totalTime;
            var pair = _pairs[switchIndex];
            return pair.StartSwitching(totalTime);
        }

        public BaseCard GetCardByIndex(int cardIndex) 
        { 
            return _pairs[cardIndex].Main;
        }

        private bool CanPlayCard(BaseCard card, GamePlayer player)
        {
            return _battleProvider.IsBattleStarted && player.HealthParam.ActualValue >= card.ManaCost.ActualValue && !_cardsCooldownProvider.IsCardBlocked(card) && !_pairs.First(e => e.Main == card).IsSwitching;
        }
    }
}
