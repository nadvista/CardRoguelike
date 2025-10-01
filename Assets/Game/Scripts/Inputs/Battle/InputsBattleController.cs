using Core.Battle;
using Inputs.SimpleHandlers;
using System;

namespace Inputs.Battle
{
    public class InputsBattleController : IDisposable
    {
        private IBattleCardsController _battleCards;
        private GameplaySimpleInputsHandler _gameplaySimpleInputsHandler;

        public InputsBattleController(IBattleCardsController battleCards, GameplaySimpleInputsHandler gameplaySimpleInputsHandler)
        {
            _battleCards = battleCards;
            _gameplaySimpleInputsHandler = gameplaySimpleInputsHandler;

            _gameplaySimpleInputsHandler.OnPlayPerformed += OnPlayPerformed;
            _gameplaySimpleInputsHandler.OnSwitchPerformed += OnSwitchPerformed;
        }

        private void OnSwitchPerformed(int obj)
        {
            var switchIndex = obj - 1;
            _battleCards.SwitchCardsPair(switchIndex);
        }

        private void OnPlayPerformed(int obj)
        {
            var cardIndex = obj - 1;
            _battleCards.PlayBattleCard(cardIndex);
        }

        public void Dispose()
        {
            _gameplaySimpleInputsHandler.OnPlayPerformed -= OnPlayPerformed;
            _gameplaySimpleInputsHandler.OnSwitchPerformed -= OnSwitchPerformed;
        }
    }
}