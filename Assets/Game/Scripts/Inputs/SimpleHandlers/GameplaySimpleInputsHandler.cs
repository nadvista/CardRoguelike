using Core.Inputs;
using System;
using UnityEngine.InputSystem;

namespace Inputs.SimpleHandlers
{
    public class GameplaySimpleInputsHandler : IInputsHandler
    {
        private GameControls _gameControls;

        public event Action<int> OnPlayPerformed;
        public event Action<int> OnSwitchPerformed;

        public GameplaySimpleInputsHandler()
        {
            _gameControls = new GameControls();
            _gameControls.Enable();
            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
            _gameControls.Dispose();
        }

        private void SubscribeEvents()
        {
            _gameControls.Gameplay.Switch1.performed += Switch1Performed;
            _gameControls.Gameplay.Switch2.performed += Switch2Performed;
            _gameControls.Gameplay.Switch3.performed += Switch3Performed;
            _gameControls.Gameplay.Switch4.performed += Switch4Performed;

            _gameControls.Gameplay.Play1.performed += Play1Performed;
            _gameControls.Gameplay.Play2.performed += Play2Performed;
            _gameControls.Gameplay.Play3.performed += Play3Performed;
            _gameControls.Gameplay.Play4.performed += Play4Performed;
        }
        private void UnsubscribeEvents()
        {
            _gameControls.Gameplay.Switch1.performed -= Switch1Performed;
            _gameControls.Gameplay.Switch2.performed -= Switch2Performed;
            _gameControls.Gameplay.Switch3.performed -= Switch3Performed;
            _gameControls.Gameplay.Switch4.performed -= Switch4Performed;

            _gameControls.Gameplay.Play1.performed -= Play1Performed;
            _gameControls.Gameplay.Play2.performed -= Play2Performed;
            _gameControls.Gameplay.Play3.performed -= Play3Performed;
            _gameControls.Gameplay.Play4.performed -= Play4Performed;
        }

        private void Play4Performed(InputAction.CallbackContext context)
        {
            OnPlayPerformed?.Invoke(4);
        }

        private void Play3Performed(InputAction.CallbackContext context)
        {
            OnPlayPerformed?.Invoke(3);
        }

        private void Play2Performed(InputAction.CallbackContext context)
        {
            OnPlayPerformed?.Invoke(2);
        }

        private void Play1Performed(InputAction.CallbackContext context)
        {
            OnPlayPerformed?.Invoke(1);
        }

        private void Switch4Performed(InputAction.CallbackContext context)
        {
            OnSwitchPerformed?.Invoke(4);
        }

        private void Switch3Performed(InputAction.CallbackContext context)
        {
            OnSwitchPerformed?.Invoke(3);
        }

        private void Switch2Performed(InputAction.CallbackContext context)
        {
            OnSwitchPerformed?.Invoke(2);
        }

        private void Switch1Performed(InputAction.CallbackContext context)
        {
            OnSwitchPerformed?.Invoke(1);
        }
    }
}