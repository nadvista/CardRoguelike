using Core.Battle;
using Inputs.SimpleHandlers;
using System;

namespace Inputs.Battle
{
    public class InputsBattleController : IDisposable
    {
        private IBattleProvider _battleProvider;
        private GameplaySimpleInputsHandler _gameplaySimpleInputsHandler;

        public InputsBattleController(IBattleProvider battleProvider, GameplaySimpleInputsHandler gameplaySimpleInputsHandler)
        {
            _battleProvider = battleProvider;
            _gameplaySimpleInputsHandler = gameplaySimpleInputsHandler;

            _gameplaySimpleInputsHandler.OnPlayPerformed += OnPlayPerformed;
            _gameplaySimpleInputsHandler.OnSwitchPerformed += OnSwitchPerformed;
        }

        private void OnSwitchPerformed(int obj)
        {
            
        }

        private void OnPlayPerformed(int obj)
        {
            var cardIndex = obj - 1;
            _battleProvider.PlayBattleCard(cardIndex);
        }

        public void Dispose()
        {
            _gameplaySimpleInputsHandler.OnPlayPerformed -= OnPlayPerformed;
            _gameplaySimpleInputsHandler.OnSwitchPerformed -= OnSwitchPerformed;
        }
    }
}