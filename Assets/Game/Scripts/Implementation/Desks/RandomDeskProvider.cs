using Core.Desk;
using Core.Tools;

namespace Implementation.Desks
{
    public class RandomDeskProvider : IDesksProvider
    {
        private DesksDatabase _database;

        public RandomDeskProvider(DesksDatabase database)
        {
            _database = database;
        }

        public CardsDesk GetDesk()
        {
            return _database.Desks.RandomElement();
        }
    }
}
