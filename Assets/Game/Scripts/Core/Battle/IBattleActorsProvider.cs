using Core.Actors.Enemies;
using Core.Actors.Players;
using Core.Params;

namespace Core.Battle
{
    public interface IBattleActorsProvider
    {
        public GamePlayer CurrentPlayer { get; }
        public GameEnemy CurrentEnemy { get; }

        public Param PlayerMana { get; }
        public Param EnemyHeahth { get; }
    }
}
