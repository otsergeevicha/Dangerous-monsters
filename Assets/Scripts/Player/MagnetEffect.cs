using Loots;
using Plugins.MonoCache;
using UnityEngine;

namespace Player
{
    public class MagnetEffect : MonoCache
    {
        [HideInInspector] [SerializeField] private Hero _hero;
        
        private readonly float _duringSeconds = 10f;
        private float _timer;

        private void OnValidate() => 
            _hero ??= Get<Hero>();

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Money money))
            {
                _hero.ApplyMoney(money.CurrentNominal);
                money.PickUp();
            }
        }

        protected override void UpdateCached()
        {
            if (!isActiveAndEnabled)
                return;

            if (_timer > 0)
                _timer -= Time.deltaTime;
            else
                InActive();
        }

        public void OnActive()
        {
            _timer = _duringSeconds;
            gameObject.SetActive(true);
        }

        public void InActive() => 
            gameObject.SetActive(false);
    }
}