using Core.Battle;
using Core.ScoreCounting;
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
        private IBattleProvider _battle;

        [Inject]
        private void Construct(ScoreCounter counter, IBattleProvider battle)
        {
            _scoreCounter = counter;
            _battle = battle;
        }

        private void Update()
        {
            var time = _battle.TimeFromLastStep;
            var fillPc = Mathf.Max(0, _scoreCounter.BonusTime - time) / _scoreCounter.BonusTime;
            fillImage.fillAmount = fillPc;
        }
    }
}
