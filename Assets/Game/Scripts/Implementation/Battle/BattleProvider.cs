using Core.Actors;
using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Battle;
using Core.Cards;
using Core.Desk;
using Core.Params;
using Core.ScoreCounting;
using Core.Steps;
using Core.Tools.Timer;
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
        private GlobalStepsCounter _stepCounter;

        private ModifiersPool _modifiersPool;

        private GameTimer _gameTimer;
        private ScoreCounter _scoreCounter;
        private ICardsCooldownProvider _cardsCooldownProvider;

        public event Action<GameActor, GameActor, CardsDesk> OnBattleStart;
        public event Action<BattleResult> OnBattleEnd;
        public event Action<BaseCard, PlayCardResult> OnPlayCard;

        public bool IsBattleStarted { get; private set; }
        public GamePlayer CurrentPlayer { get; private set; }
        public GameEnemy CurrentEnemy { get; private set; }
        public CardsDesk CurrentCardsDesk { get; private set; }

        public float TimeFromLastStep => _gameTimer.CurrentTimeSeconds;

        public Param PlayerMana => CurrentPlayer.HealthParam;

        public Param EnemyHeahth => CurrentEnemy.HealthParam;

        public BattleProvider(IDesksProvider deskProvider, IPlayerProvider player, IEnemyProvider enemyProvider, 
            GlobalStepsCounter stepCounter, 
            TimersPool gameTimersPool, 
            ScoreCounter scoreCounter, 
            ModifiersPool modifiersPool, 
            ICardsCooldownProvider cooldownProvider)
        {
            _deskProvider = deskProvider;
            _playerProvider = player;
            _enemyProvider = enemyProvider;
            _gameTimer = gameTimersPool.Get();
            _scoreCounter = scoreCounter;
            _modifiersPool = modifiersPool;
            _stepCounter = stepCounter;
            _cardsCooldownProvider = cooldownProvider;
        }

        public void PlayBattleCard(BaseCard card)
        {
            if (!IsBattleStarted)
            {
                OnPlayCard?.Invoke(card, PlayCardResult.Unsuccess);
                return;
            }
            if (!CurrentCardsDesk.Cards.Contains(card))
            {
                Debug.LogError("it is impossible to play a card that is not in the deck");
                OnPlayCard?.Invoke(card, PlayCardResult.Unsuccess);
                return;
            }
            if (!CanPlayCard(card))
            {
                OnPlayCard?.Invoke(card, PlayCardResult.Unsuccess);
                return;
            }
            var time = _gameTimer.CurrentTimeSeconds;
            var timeBonus = _scoreCounter.CalculateScore(time);

            var subtractMod = _modifiersPool.GetSubtractModifier(int.MaxValue, card.ManaCost.ActualValue);

            PlayerMana.ApplyModifier(subtractMod);

            foreach (var action in card.Actions)
                action.DoAction(CurrentPlayer, CurrentEnemy, timeBonus);

            _stepCounter.NewStep();
            _gameTimer.Start();
            _cardsCooldownProvider.BlockCard(card);

            OnPlayCard?.Invoke(card, PlayCardResult.Success);
        }
        public void PlayBattleCard(int deskCardIndex)
        {
            if (!IsBattleStarted)
                return;
            if (deskCardIndex >= CurrentCardsDesk.Cards.Count)
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
            return PlayerMana.ActualValue >= card.ManaCost.ActualValue && !_cardsCooldownProvider.IsCardBlocked(card);
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
