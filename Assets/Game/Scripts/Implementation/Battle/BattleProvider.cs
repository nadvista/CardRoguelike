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

namespace Implementation.Battle
{
    public class BattleProvider : IBattleProvider, IDisposable
    {
        private const float TOTAL_CARDS_SWITCHING_TIME = 0.3f;

        private IDesksProvider _deskProvider;
        private IPlayerProvider _playerProvider;
        private IEnemyProvider _enemyProvider;
        private GlobalStepsCounter _stepCounter;

        private ModifiersPool _modifiersPool;
        private BattleCardsController _cardsController;

        private GameTimer _gameTimer;
        private ScoreCounter _scoreCounter;

        public event Action<GamePlayer, GameEnemy, CardsDesk> OnBattlePrepared;
        public event Action OnBattleStarted;
        public event Action<BattleResult> OnBattleEnd;
        public event Action<BaseCard, PlayCardResult> OnPlayCard;
        public event Action<int, float> OnCardSwitchingStarted;

        public bool IsBattleStarted { get; private set; }
        public bool IsBattlePrepared { get; private set; }  
        public GamePlayer CurrentPlayer { get; private set; }
        public GameEnemy CurrentEnemy { get; private set; }
        public CardsDesk CurrentCardsDesk { get; private set; }

        public float TimeFromLastStepSeconds => _gameTimer.CurrentTimeSeconds;

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

            _cardsController = new BattleCardsController(cooldownProvider, this, gameTimersPool);
        }

        public void PlayBattleCard(int deskCardIndex)
        {
            var card = _cardsController.GetCardByIndex(deskCardIndex);
            if (!IsBattleStarted)
            {
                OnPlayCard?.Invoke(card, PlayCardResult.Unsuccess);
            }

            var time = _gameTimer.CurrentTimeSeconds;
            var timeBonus = _scoreCounter.CalculateScore(time);

            var result = _cardsController.PlayBattleCard(deskCardIndex, CurrentPlayer, CurrentEnemy, timeBonus);

            if(result == PlayCardResult.Success)
            {
                _stepCounter.NewStep();
                _gameTimer.Start();
            }

            OnPlayCard?.Invoke(card, result);
        }
        public void SwitchCardsPair(int number)
        {
            var timeToSwitch = _cardsController.SwitchCards(number, TOTAL_CARDS_SWITCHING_TIME);
            OnCardSwitchingStarted?.Invoke(number, timeToSwitch);
        }

        public void StopBattle()
        {
            StopBattle(BattleResult.Loose);
        }
        public void PrepareBattle()
        {
            if (IsBattleStarted)
                StopBattle();

            UnsubscribeBattlePropsEvents();
            ResolveBattleProps();
            ResetBattleProps();
            SubscribeBattlePropsEvents();
            _cardsController.SetupDesk(CurrentCardsDesk);
            IsBattlePrepared = true;
            OnBattlePrepared?.Invoke(CurrentPlayer, CurrentEnemy, CurrentCardsDesk);
        }
        public void StartBattle()
        {
            if (!IsBattlePrepared)
                return;
            _gameTimer.Start();

            IsBattleStarted = true;
            OnBattleStarted?.Invoke();
        }

        public void Dispose()
        {
            StopBattle();
            _cardsController.Dispose();
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
            _gameTimer.ResetTime();
            _gameTimer.Stop();
            UnsubscribeBattlePropsEvents();
            IsBattleStarted = false;
            IsBattlePrepared = false;
            OnBattleEnd?.Invoke(result);
            _cardsController.Reset();
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
