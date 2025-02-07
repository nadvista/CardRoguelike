namespace Core.Tools.Pool
{
    public interface IPoolElement
    {
        public abstract bool IsInactive { get; }

        public abstract void OnReturnToPool();
        public abstract void OnTakeFromPool();
    }
}
