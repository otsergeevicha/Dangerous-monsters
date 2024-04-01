using Plugins.MonoCache;
using UnityEngine;

namespace Bank
{
    public class Money : MonoCache
    {
        public int CurrentNominal { get; private set; } = 1;

        public void OnActive(int enemyId, Vector3 newPosition)
        {
            if (newPosition.y < 0) 
                newPosition.y = 0;

            transform.position = newPosition;
            SetNominal(enemyId);
            gameObject.SetActive(true);
        }

        public void InActive()
        {
            ResetNominal();
            gameObject.SetActive(false);
        }

        private void SetNominal(int enemyId) => 
            CurrentNominal = (enemyId + 1) * 3;

        private void ResetNominal() => 
            CurrentNominal = 1;
    }
}