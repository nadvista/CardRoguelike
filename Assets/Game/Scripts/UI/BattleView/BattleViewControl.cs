using Core.Actors;
using Core.Battle;
using Core.Desk;
using System;
using Ui.Actors;
using Ui.Cards;
using Ui.ScoreCounting;
using UnityEngine;
using Zenject;

namespace Ui.BattleView
{
    public class BattleViewControl : MonoBehaviour
    {
        [SerializeField]
        private ActorView playerView;

        [SerializeField]
        private ActorView enemyView;

        [SerializeField]
        private DeskView deskView;

        [SerializeField]
        private ScoreCounterView scoreCounterView;

        private IBattleProvider _battle;

        [Inject]
        private void Construct(IBattleProvider battle)
        {
            _battle = battle;
        }
        private void Awake()
        {
            _battle.OnBattleStart += OnBattleStart;
        }

        private void Start()
        {
            _battle.StartBattle();
        }

        private void OnBattleStart(GameActor actor1, GameActor actor2, CardsDesk desk)
        {
            playerView.Setup(actor1);
            enemyView.Setup(actor2);
            deskView.Fill(desk.Cards);
        }
    }
}
