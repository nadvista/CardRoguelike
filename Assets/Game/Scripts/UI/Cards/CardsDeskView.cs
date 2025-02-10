using Core.Actors;
using Core.Battle;
using Core.Desk;
using Core.Inputs;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Ui.Cards
{
    public class CardsDeskView : MonoBehaviour
    {
        [SerializeField]
        private List<CardsPairContainer> pairContainers;

        private IBattleProvider _battleProvider;

        private bool _isBattleStarted;

        [Inject]
        private void Construct(IBattleProvider battle)
        {
            _battleProvider = battle;
        }
        private void Awake()
        {
            _battleProvider.OnBattleStart += OnBattleStart;
            _battleProvider.OnBattleEnd += OnBattleEnd;

            _battleProvider.OnCardSwitchingStarted += OnSwitchPerformed;
        }

        private void OnDestroy()
        {
            _battleProvider.OnBattleStart -= OnBattleStart;
            _battleProvider.OnBattleEnd -= OnBattleEnd;

            _battleProvider.OnCardSwitchingStarted -= OnSwitchPerformed;
        }

        private void OnSwitchPerformed(int obj, float time)
        {
            if (!_isBattleStarted)
                return;
            pairContainers[obj].Switch(time);
        }

        private void OnBattleEnd(BattleResult result)
        {
            _isBattleStarted = false;
        }

        private void OnBattleStart(GameActor actor1, GameActor actor2, CardsDesk desk)
        {
            _isBattleStarted = true;

            var pairsCount = Mathf.CeilToInt(desk.Cards.Count / 2);
            if(pairsCount > pairContainers.Count)
            {
                Debug.LogError("Количество пар карт превышает количество контейнеров");
            }
            var cycleCount = Mathf.Min(pairsCount, pairContainers.Count);
            for(int pair = 0; pair < cycleCount; pair++)
            {
                var container = pairContainers[pair];

                var card1Index = pair * 2;
                var card2Index = pair * 2 + 1;

                var card1 = desk.Cards[card1Index];
                var card2 = desk.Cards.Count > card2Index ? desk.Cards[card2Index] : card1;

                container.SetupPair(card1, card2);
            }
        }
    }
}
