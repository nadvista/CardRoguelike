using Core.Tools.Pool;
using System;
using UnityEngine;
using Zenject;

namespace Core.Tools.Timer
{

    public class GameTimer : ITickable, IPoolElement
    {
        public float CurrentTimeSeconds { get; private set; }
        public bool IsWorking => IsStarted && !IsPaused;
        public bool IsPaused { get; private set; }
        public bool IsStarted { get; private set; }

        public bool IsInactive { get; private set; }

        private float _waitTime;
        private Action _onStopCallback;

        public event Action OnTimerStart;
        public event Action OnTimerStop;
        public event Action OnTimerPause;
        public event Action OnTimerResume;
        public event Action OnTick;

        public void Start(float waitTime = float.MaxValue, Action onStopCallback = null)
        {
            _waitTime = waitTime;
            IsPaused = false;
            IsStarted = true;

            CurrentTimeSeconds = 0;
            _onStopCallback = onStopCallback;
            OnTimerStart?.Invoke();
        }

        public void Stop()
        {
            IsStarted = false;
            IsPaused = false;

            OnTimerStop?.Invoke();
        }
        public void ResetTime()
        {
            CurrentTimeSeconds = 0f;
        }
        public void Pause()
        {
            if (!IsStarted)
                return;

            IsPaused = true;

            OnTimerPause?.Invoke();
        }

        public void Resume()
        {
            if (!IsStarted || !IsPaused)
                return;

            IsPaused = false;

            OnTimerResume?.Invoke();
        }

        public void Tick()
        {
            if (!IsWorking)
                return;

            CurrentTimeSeconds += Time.deltaTime;
            OnTick?.Invoke();

            if (CurrentTimeSeconds >= _waitTime)
            {
                _onStopCallback?.Invoke();
                Stop();
            }
        }

        public void Release()
        {
            Stop();
            IsInactive = true;
        }

        public void OnReturnToPool()
        {
            Stop();
        }

        public void OnTakeFromPool()
        {
            IsInactive = false;
        }
    }
}
