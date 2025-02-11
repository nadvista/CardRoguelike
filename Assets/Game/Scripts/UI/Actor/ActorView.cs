using Core.Actors;
using Core.Battle;
using Core.Desk;
using Core.Params;
using Core.Tools.Timer;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Ui.Params;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.Actors
{
    public class ActorView : MonoBehaviour
    {
        [SerializeField]
        private ActorViewType type;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TextMeshProUGUI nameLabel;

        [SerializeField]
        private ParamsContainerView paramsContainer;

        [SerializeField]
        private List<TextMeshProUGUI> _hitIndicators;

        private IBattleProvider _battleProvider;
        private ActorHealthChangeVisualer _healthVisualizerComponent;

        [Inject]
        private void Construct(IBattleProvider battleProvider, TimersPool timers)
        {
            _battleProvider = battleProvider;
            _healthVisualizerComponent = new ActorHealthChangeVisualer(_hitIndicators, timers);
        }
        private void Awake()
        {
            _battleProvider.OnBattleStart += OnNewBattle;
        }
        private void OnDestroy()
        {
            _battleProvider.OnBattleStart -= OnNewBattle;
        }

        private void OnNewBattle(GameActor actor1, GameActor actor2, CardsDesk desk)
        {
            switch (type)
            {
                case ActorViewType.Player:
                    Setup(actor1);
                    return;
                case ActorViewType.Enemy:
                    Setup(actor2);
                    return;
            }
        }

        public void Setup(GameActor data)
        {
            iconImage.sprite = data.Data.Image;

            nameLabel.text = data.Data.Name;

            var allParams = new List<Param>(data.AllParams);
            allParams.Add(data.HealthParam);

            paramsContainer.Fill(allParams);
            _healthVisualizerComponent.Setup(data);
        }
    }
}
