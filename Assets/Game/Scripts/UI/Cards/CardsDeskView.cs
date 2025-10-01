using Core.Actors;
using Core.Battle;
using Core.Desk;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Ui.Cards
{
    public class CardsDeskView : MonoBehaviour
    {
        [SerializeField]
        private List<CardsPairContainer> pairContainers;

        private IBattleStopController _battleStop;
        private IBattlePrepareController _battlePrepare;
        private IBattleCardsController _battleCards;

        private bool _isBattleStarted;

        [Inject]
        private void Construct(IBattleStopController battleStop, IBattlePrepareController battlePrepare, IBattleCardsController battleCards)
        {
            _battleStop = battleStop;
            _battlePrepare = battlePrepare;
            _battleCards = battleCards;
        }
        private void Awake()
        {
            _battlePrepare.OnBattlePrepared += OnBattleStart;
            _battleStop.OnBattleEnd += OnBattleEnd;

            _battleCards.OnCardSwitchingStarted += OnSwitchPerformed;
        }

        private void OnDestroy()
        {
            _battlePrepare.OnBattlePrepared -= OnBattleStart;
            _battleStop.OnBattleEnd -= OnBattleEnd;

            _battleCards.OnCardSwitchingStarted -= OnSwitchPerformed;
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
            foreach (var pair in pairContainers)
                pair.SetPositionToDefault();
        }

        private void OnBattleStart(GameActor actor1, GameActor actor2, CardsDesk desk)
        {
            _isBattleStarted = true;

            var pairsCount = Mathf.CeilToInt(desk.Cards.Count / 2);
            if (pairsCount > pairContainers.Count)
            {
                Debug.LogError("Количество пар карт превышает количество контейнеров");
            }
            var cycleCount = Mathf.Min(pairsCount, pairContainers.Count);
            for (int pair = 0; pair < cycleCount; pair++)
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
