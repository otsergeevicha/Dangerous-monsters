using Plugins.MonoCache;

namespace Turret
{
    public class CartridgeBox : MonoCache
    {
        public void OnActive() =>
            gameObject.SetActive(true);

        public void InActive() =>
            gameObject.SetActive(false);
    }
}