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
            CurrentPlayer = player;
        }
        private void OnEnemyChange(Actor enemy)
        {
            CurrentEnemy = enemy;
        }
    }
}
