using Plugins.MonoCache;
using UnityEngine;

namespace Player
{
    public class AmmoBox : MonoCache
    {
        public void OnActive() =>
            gameObject.SetActive(true);

        public void InActive() =>
            gameObject.SetActive(false);

        public void SetPosition(Vector3 newPosition) =>
            transform.position = newPosition;
    }
}