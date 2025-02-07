namespace Core.Tools.Pool
{
    public interface IPoolFabric<PoolElementType> where PoolElementType : IPoolElement
    {
        public PoolElementType CreateNew();
    }
}
