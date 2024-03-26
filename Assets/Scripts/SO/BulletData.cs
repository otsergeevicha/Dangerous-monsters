using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewBulletData", menuName = "Ammo/BulletData", order = 1)]
    public class BulletData : ScriptableObject
    {
        [Range(1,10)]
        public int RadiusExplosion = 5;

        [Range(1, 200)] 
        public int MissileDamage = 5;
    }
}