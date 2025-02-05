using Core.Actors;
using Core.Cards;
using Core.Desk;
using Core.Params;
using Core.Tools;
using System;
using UnityEngine;

namespace Core.Battle
{
    public class BattleProvider
    {
        private DeskProvider _deskProvider;
        private ActorsProvider _actorsProvider;

        private Timer _gameTimer;
        private ScoreCounter _scoreCounter;

        public event Action<Actor, Actor, CardsDesk> OnBattleStart;
        public event Action<BattleResult> OnBattleEnd;
        public event Action<PlayCardResult> OnPlayCard;

        public bool IsBattleStarted { get; private set; }
        public Actor CurrentPlayer { get; private set; }
        public Actor CurrentEnemy { get; private set; }
        public CardsDesk CurrentCardsDesk { get; private set; }

        private Param _playerManaParam => CurrentPlayer.HealthParam;

        public BattleProvider(DeskProvider deskProvider, ActorsProvider actorsProvider, Timer gameTimer, ScoreCounter scoreCounter)
        {
            _deskProvider = deskProvider;
            _actorsProvider = actorsProvider;
            _gameTimer = gameTimer;
            _scoreCounter = scoreCounter;
        }

        public void PlayBattleCard(BaseCard card)
        {
            if (!IsBattleStarted)
            {
                OnPlayCard?.Invoke(PlayCardResult.Unsuccess);
                return;
            }
            if (!CurrentCardsDesk.Cards.Contains(card))
            {
                Debug.LogError("it is impossible to play a card that is not in the deck");
                OnPlayCard?.Invoke(PlayCardResult.Unsuccess);
                return;
            }
            if (!CanPlayCard(card))
            {
                OnPlayCard?.Invoke(PlayCardResult.Unsuccess);
                return;
            }
            var time = _gameTimer.CurrentTime;
            var timeBonus = _scoreCounter.CalculateScore(time);

            _playerManaParam.ApplyModifier(new SubtractModifier(card.ManaCost.ActualValue, int.MaxValue));

            foreach (var action in card.Actions)
                action.DoAction(CurrentPlayer, CurrentEnemy, timeBonus);

            GlobalStepsManager.NewStep();
            _gameTimer.StartTimer();
            OnPlayCard?.Invoke(PlayCardResult.Success);
        }
        public void PlayBattleCard(int deskCardIndex)
        {
            if (!IsBattleStarted)
                return;

            var card = CurrentCardsDesk.Cards[deskCardIndex];
            
            PlayBattleCard(card);
        }

        public void StopBattle()
        {
            StopBattle(BattleResult.Loose);
        }
        public void StartBattle(Actor player = null, Actor enemy = null, CardsDesk desk = null) 
        {
            if (IsBattleStarted)
                StopBattle();

            UnsubscribeBattlePropsEvents();
            ResolveBattleProps(player, enemy, desk);
            ResetBattleProps();
            SubscribeBattlePropsEvents();
            _gameTimer.StartTimer();

            IsBattleStarted = true;
            OnBattleStart?.Invoke(CurrentPlayer, CurrentEnemy, CurrentCardsDesk);
        }

        private void UnsubscribeBattlePropsEvents()
        {
            if(CurrentPlayer != null)
                CurrentPlayer.OnDead -= OnPlayerManaLeft;
            if (CurrentEnemy != null)
                CurrentEnemy.OnDead -= OnEnemyDead;
        }
        private void SubscribeBattlePropsEvents()
        {
            if (CurrentPlayer != null)
                CurrentPlayer.OnDead += OnPlayerManaLeft;
            if (CurrentEnemy != null)
                CurrentEnemy.OnDead += OnEnemyDead;
        }
        private void ResolveBattleProps(Actor player = null, Actor enemy = null, CardsDesk desk = null)
        {
            if (player == null)
                player = _actorsProvider.CurrentPlayer;
            if (enemy == null)
                enemy = _actorsProvider.CurrentEnemy;
            if (desk == null)
                desk = _deskProvider.CurrentDesk;

            CurrentPlayer = player;
            CurrentEnemy = enemy;
            CurrentCardsDesk = desk;
        }
        private void ResetBattleProps()
        {
            CurrentPlayer?.Initialize();
            CurrentEnemy?.Initialize();
        }
        private void StopBattle(BattleResult result)
        {
            _gameTimer.StopTimer();

            IsBattleStarted = false;
            OnBattleEnd?.Invoke(result);
        }

        private bool CanPlayCard(BaseCard card)
        {
            return _playerManaParam.ActualValue >= card.ManaCost.ActualValue;
        }

        private void OnEnemyDead()
        {
            StopBattle(BattleResult.Win);
        }
        private void OnPlayerManaLeft()
        {
            StopBattle(BattleResult.Loose);
        }
    }
}
