using System;
using System.Collections;
using UnityEngine;

namespace Core.Tools
{
    public class Timer : MonoBehaviour
    {
        public float CurrentTime { get; private set; }
        public bool IsWorking { get; private set; }

        private Coroutine _timerCoroutine;
        private float _maxTime;

        public event Action OnStart;
        public event Action<float> OnStop;

        public void StartTimer(float maxTime = float.MaxValue)
        {
            StopTimer();
            _maxTime = maxTime;
            IsWorking = true;
            _timerCoroutine = StartCoroutine(TimerCoroutine());
            OnStart?.Invoke();
        }

        public float StopTimer()
        {
            if (!IsWorking)
                return 0;

            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);
            var result = CurrentTime;
            CurrentTime = 0;
            IsWorking = false;

            OnStop?.Invoke(result);
            return result;
        }

        private IEnumerator TimerCoroutine()
        {
            var wait = new WaitForEndOfFrame();
            while (IsWorking && CurrentTime < _maxTime)
            {
                yield return wait;
                CurrentTime += Time.deltaTime;
            }
            StopTimer();
        }
    }
}
