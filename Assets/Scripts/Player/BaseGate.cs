using Plugins.MonoCache;
using UnityEngine;

namespace Player
{
    public class BaseGate : MonoCache
    {
        private bool _heroOnBase = true;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero hero))
            {
                _heroOnBase = !_heroOnBase;
                hero.SetActualRunHash(_heroOnBase);
            }
        }
    }
}