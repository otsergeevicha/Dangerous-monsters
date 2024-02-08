using Plugins.MonoCache;

namespace Player
{
    public class CartridgeBox : MonoCache
    {
        public void OnActive() =>
            gameObject.SetActive(true);

        public void InActive() =>
            gameObject.SetActive(false);
    }
}