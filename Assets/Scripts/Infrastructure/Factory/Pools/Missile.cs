using Plugins.MonoCache;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class Missile : MonoCache
    {
        private AmmoData _ammoData;

        public void Construct(AmmoData ammoData)
        {
            _ammoData = ammoData;
        }

        public void OnActive()
        {
            throw new System.NotImplementedException();
        }
        
        public void InActive() => 
            gameObject.SetActive(false);
    }
}