using Core.Tools.Pool;
using Zenject;

namespace Core.Tools.Timer
{
    public class TimersPool : ObjectsPool<GameTimer>, ITickable
    {
        public TimersPool(IPoolFabric<GameTimer> fabric) : base(fabric)
        {
        }

        public void Tick()
        {
            foreach (var timer in _workingElements)
                timer.Tick();
        }
    }
}
