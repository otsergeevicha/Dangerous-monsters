using Plugins.MonoCache;
using SO;

namespace Infrastructure.Factory.Pools
{
    enum EnemyId
    {
        ZeroLevel,
        OneLevel,
        TwoLevel,
        ThreeLevel,
        FourLevel,
        FiveLevel,
        SixLevel,
        SevenLevel,
        EightLevel,
        NineLevel
    }

    public abstract class Enemy : MonoCache
    {
        public abstract int GetId();
        public abstract void Construct(EnemyData enemyData);
        public abstract void OnActive();
        public abstract void InActive();
    }
}