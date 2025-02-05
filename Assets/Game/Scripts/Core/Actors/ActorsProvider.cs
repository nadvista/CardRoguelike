using Core.Tools;
using System;

namespace Core.Actors
{
    public class ActorsProvider
    {
        private ActorsDatabase _database;
        private DataProvider<Actor> _playerProvider;
        private DataProvider<Actor> _enemiesProvider;

        public Actor CurrentPlayer { get; private set; }
        public Actor CurrentEnemy { get; private set; }

        public event Action OnManaLeft;
        public event Action OnEnemyDead;
        public event Action<Actor> PlayerChange;
        public event Action<Actor> EnemyChange;
        public ActorsProvider(ActorsDatabase database)
        {
            _database = database;
            _playerProvider = new DataProvider<Actor>(database.PlayerList, false);
            _enemiesProvider = new DataProvider<Actor>(database.EnemiesList, false);

            _playerProvider.Changed += OnPlayerChange;
            _enemiesProvider.Changed += OnEnemyChange;

            SelectNewPlayer();
            SelectNewEnemy();
        }

        public void SelectNewPlayer()
        {
            _playerProvider.GetNew();
        }
        public void SelectNewEnemy()
        {
            _enemiesProvider.GetNew();
        }

        private void OnPlayerChange(Actor player)
        {
            if (CurrentPlayer != null)
            {
                CurrentPlayer.OnDead -= OnPlayerManaLeft;
            }
            CurrentPlayer = player;
            CurrentPlayer.OnDead += OnPlayerManaLeft;

            PlayerChange?.Invoke(CurrentPlayer);
        }
        private void OnEnemyChange(Actor enemy)
        {
            if (CurrentEnemy != null)
            {
                CurrentEnemy.OnDead -= OnEnemyHealthLeft;
            }
            CurrentEnemy = enemy;
            CurrentEnemy.OnDead += OnEnemyHealthLeft;

            EnemyChange?.Invoke(CurrentEnemy);
        }

        private void OnPlayerManaLeft()
        {
            OnManaLeft?.Invoke();
        }
        private void OnEnemyHealthLeft()
        {
            OnEnemyDead?.Invoke();
        }
    }
}
