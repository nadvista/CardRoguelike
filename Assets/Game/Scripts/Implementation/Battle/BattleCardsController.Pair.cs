using Core.Cards;
using Core.Tools.Timer;
using System;

namespace Implementation.Battle
{
    internal partial class BattleCardsController
    {
        private class Pair
        {
            public BaseCard Main { get; set; }
            public BaseCard Secondary { get; set; }
            public bool IsSwitching { get; private set; }

            private float _lastStopTime;
            private int _stopsCount;
            private GameTimer _switchingTimer;
            private bool _isStoppedManually;

            public event Action OnSwitchEnd;

            public Pair(BaseCard main, BaseCard secondary, GameTimer switchingTimer)
            {
                Main = main;
                Secondary = secondary;
                _switchingTimer = switchingTimer;
                _switchingTimer = switchingTimer;
            }

            public float StartSwitching(float switchingTime)
            {
                var time = switchingTime;
                var temp = Main;
                Main = Secondary;
                Secondary = temp;

                if (IsSwitching) // если произошел перевыбор во время движения
                {
                    var timeElapsed = _switchingTimer.CurrentTimeSeconds;
                    _isStoppedManually = true;
                    _switchingTimer.Stop();
                    _isStoppedManually = false;

                    _stopsCount++;
                    if (_stopsCount % 2 == 0) // двигаемся в изначальную до перевыборов сторону
                    {
                        _lastStopTime -= timeElapsed;
                        time -= _lastStopTime;
                    }
                    else
                    {
                        _lastStopTime += timeElapsed; // двигаемся в сторону, из которой изначально уходили
                        time = _lastStopTime;
                    }
                }
                _switchingTimer.Start(time, OnTimerStop);
                IsSwitching = true;
                return time;
            }
            public void ReleaseTimer()
            {
                if(_switchingTimer != null)
                    _switchingTimer.Release();
            }

            private void OnTimerStop()
            {
                if (_isStoppedManually)
                    return;

                _lastStopTime = 0;
                _stopsCount = 0;

                IsSwitching = false;
                OnSwitchEnd?.Invoke();
            }
        }
    }
}
