using Core.Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.ScoreCounting
{
    public class ScoreCounterView : MonoBehaviour
    {
        [SerializeField]
        private Image fillImage;
        private ScoreCounter _scoreCounter;
        private Timer _timer;

        [Inject]
        private void Construct(ScoreCounter counter, Timer timer)
        {
            _scoreCounter = counter;
            _timer = timer;
        }

        private void Update()
        {
            var time = _timer.CurrentTime;
            var fillPc = Mathf.Max(0, _scoreCounter.BonusTime - time) / _scoreCounter.BonusTime;
            fillImage.fillAmount = fillPc;
        }
    }
}
