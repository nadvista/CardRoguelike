using System;
using UnityEngine;
using Zenject;

namespace Core.Tools
{
    public class Timer : ITickable
    {
        public float CurrentTimeSeconds { get; private set; }
        public bool IsWorking => IsStarted && !IsPaused;
        public bool IsPaused { get; private set; }
        public bool IsStarted { get; private set; } 

        private float _waitTime;

        public event Action OnTimerStart;
        public event Action OnTimerStop;
        public event Action OnTimerPause;
        public event Action OnTimerResume;
        public event Action OnTick;

        public void Start(float waitTime = float.MaxValue)
        {
            _waitTime = waitTime;
            IsPaused = false;
            IsStarted = true;

            CurrentTimeSeconds = 0;

            OnTimerStart?.Invoke();
        }

        public void Stop()
        {
            IsStarted = false;
            IsPaused = false;

            OnTimerStop?.Invoke();
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
            if(!IsStarted || !IsPaused) 
                return;

            IsPaused = false;

            OnTimerResume?.Invoke();
        }

        public void Tick()
        {
            if(!IsWorking) 
                return;

            CurrentTimeSeconds += Time.deltaTime;
            OnTick?.Invoke();

            if(CurrentTimeSeconds >= _waitTime)
                Stop();
        }
    }
}
