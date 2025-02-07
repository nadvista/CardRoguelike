using Core.Tools.Pool;

namespace Core.Tools.Timer
{
    public class TimersFabric : IPoolFabric<GameTimer>
    {
        public GameTimer CreateNew()
        {
            return new GameTimer();
        }
    }
}
