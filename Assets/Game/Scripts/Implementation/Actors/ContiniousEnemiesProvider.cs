using Core.Actors.Enemies;
using Core.Tools;

namespace Implementation.Actors
{
    public class ContiniousEnemiesProvider : IEnemyProvider
    {
        private ActorsDatabase _database;

        public ContiniousEnemiesProvider(ActorsDatabase database)
        {
            _database = database;
        }

        public GameEnemy GetEnemy()
        {
            return _database.EnemiesList.RandomElement();

        }
    }
}
