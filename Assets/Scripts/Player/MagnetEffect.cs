using Loots;
using Plugins.MonoCache;
using UnityEngine;

namespace Player
{
    public class MagnetEffect : MonoCache
    {
        private readonly float _duringSeconds = 10f;
        private float _timer;
        private Hero _hero;

        public void Construct(Hero hero) => 
            _hero = hero;

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
            gameObject.SetActive(true);
            _timer = _duringSeconds;
        }

        public void InActive() => 
            gameObject.SetActive(false);
    }
}