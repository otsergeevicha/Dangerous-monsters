using Plugins.MonoCache;
using UnityEngine;

namespace Workers
{
    public class Worker : MonoCache
    {
        public void OnActive()
        {
            gameObject.SetActive(true);
        }

        public void InActive()
        {
            gameObject.SetActive(false);
        }

        public void FollowHero()
        {
            throw new System.NotImplementedException();
        }
    }
}