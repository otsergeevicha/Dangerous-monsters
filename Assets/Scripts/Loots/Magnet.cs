using System;
using Modules;
using Plugins.MonoCache;

namespace Loots
{
    public class Magnet : MonoCache, ILoot
    {

        public void OnActive()
        {
            gameObject.SetActive(true);
        }

        public void Open(Action opened)
        {
            opened?.Invoke();
        }

        public void InActive()
        {
            gameObject.SetActive(false);
        }
    }
}