using Core.Actors;
using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Battle;
using Core.Cards;
using Core.Desk;
using Core.Params;
using Core.ScoreCounting;
using Core.Tools;
using Implementation.Params.Modifiers;
using System;
using UnityEngine;

namespace Implementation.Battle
{
    public class BattleProvider : IBattleProvider, IDisposable
    {
        private IDesksProvider _deskProvider;
        private IPlayerProvider _playerProvider;
        private IEnemyProvider _enemyProvider;

        private Timer _gameTimer;
        private ScoreCounter _scoreCounter;

        public event Action<GameActor, GameActor, CardsDesk> OnBattleStart;
        public event Action<BattleResult> OnBattleEnd;
        public event Action<PlayCardResult> OnPlayCard;

        public bool IsBattleStarted { get; private set; }
        public GamePlayer CurrentPlayer { get; private set; }
        public GameEnemy CurrentEnemy { get; private set; }
        public CardsDesk CurrentCardsDesk { get; private set; }

        public float TimeFromLastStep => _gameTimer.CurrentTimeSeconds;

        public Param PlayerMana => CurrentPlayer.HealthParam;

        public Param EnemyHeahth => CurrentEnemy.HealthParam;

        public BattleProvider(IDesksProvider deskProvider, IPlayerProvider player, IEnemyProvider enemyProvider, Timer gameTimer, ScoreCounter scoreCounter)
        {
            _deskProvider = deskProvider;
            _playerProvider = player;
            _enemyProvider = enemyProvider;
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
            var time = _gameTimer.CurrentTimeSeconds;
            var timeBonus = _scoreCounter.CalculateScore(time);

            PlayerMana.ApplyModifier(new SubtractModifier(card.ManaCost.ActualValue, int.MaxValue));

            foreach (var action in card.Actions)
                action.DoAction(CurrentPlayer, CurrentEnemy, timeBonus);

            GlobalStepsManager.NewStep();
            _gameTimer.Start();
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
        public void StartBattle()
        {
            if (IsBattleStarted)
                StopBattle();

            UnsubscribeBattlePropsEvents();
            ResolveBattleProps();
            ResetBattleProps();
            SubscribeBattlePropsEvents();
            _gameTimer.Start();

            IsBattleStarted = true;
            OnBattleStart?.Invoke(CurrentPlayer, CurrentEnemy, CurrentCardsDesk);
        }

        public void Dispose()
        {
            StopBattle();
        }

        private void UnsubscribeBattlePropsEvents()
        {
            if (CurrentPlayer != null)
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
        private void ResolveBattleProps()
        {
            CurrentPlayer = _playerProvider.GetPlayer();
            CurrentEnemy = _enemyProvider.GetEnemy();
            CurrentCardsDesk = _deskProvider.GetDesk();
        }
        private void ResetBattleProps()
        {
            CurrentPlayer?.Initialize();
            CurrentEnemy?.Initialize();
        }
        
        private void StopBattle(BattleResult result)
        {
            _gameTimer.Stop();
            UnsubscribeBattlePropsEvents();
            IsBattleStarted = false;
            OnBattleEnd?.Invoke(result);
        }

        private bool CanPlayCard(BaseCard card)
        {
            return PlayerMana.ActualValue >= card.ManaCost.ActualValue;
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
