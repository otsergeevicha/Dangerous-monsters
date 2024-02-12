using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewHero", menuName = "Characters/Hero", order = 1)]
    public class HeroData : ScriptableObject
    {
        [Range(3,6)]
        public int Speed = 3;

        [Range(5, 15)] 
        public int SizeBasket = 5;
        
        [HideInInspector] public int HeroIdleHash = Animator.StringToHash("Idle");
        [HideInInspector] public int HeroRunHash = Animator.StringToHash("Run");
        [HideInInspector] public int HeroRunGunHash = Animator.StringToHash("RunGun");
    }
}