using Core.Actors.Players;
using Core.Tools;
using System;

namespace Implementation.Actors
{
    public class RandomPlayerProvider : IPlayerProvider
    {
        private ActorsDatabase _database;

        public RandomPlayerProvider(ActorsDatabase database)
        {
            _database = database;
        }

        public GamePlayer GetPlayer()
        {
            return _database.PlayerList.RandomElement();
        }
    }
}
