using Core.Actors;
using Core.Battle;
using Core.Desk;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.BattleView
{
    public class BattleResultsView : MonoBehaviour
    {
        [SerializeField]
        private GameObject container;

        [SerializeField]
        private GameObject winScreen;

        [SerializeField]
        private GameObject looseScreen;

        [SerializeField]
        private Button replayButton;

        private IBattleProvider _battle;

        [Inject]
        private void Construct(IBattleProvider battle)
        {
            _battle = battle;
        }

        private void Awake()
        {
            container.SetActive(false);

            _battle.OnBattleEnd += OnBattleEnd;
            _battle.OnBattleStart += OnBattleStart;

            replayButton.onClick.AddListener(OnReplayClicked);
        }

        private void OnDestroy()
        {
            _battle.OnBattleEnd -= OnBattleEnd;
            _battle.OnBattleStart -= OnBattleStart;

            replayButton.onClick.RemoveListener(OnReplayClicked);
        }

        private void OnBattleStart(GameActor actor1, GameActor actor2, CardsDesk desk)
        {
            container.SetActive(false);
        }

        private void OnBattleEnd(BattleResult result)
        {
            container.SetActive(true);

            winScreen.SetActive(result == BattleResult.Win);
            looseScreen.SetActive(result == BattleResult.Loose);
        }

        private void OnReplayClicked()
        {
            _battle.StartBattle();
        }
    }
}
