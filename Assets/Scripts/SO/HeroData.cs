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
        
        [Header("Detection radius")]
        [Range(5f, 25f)]
        public float RadiusDetection = 5f;
        
        [HideInInspector] public int IdleHash = Animator.StringToHash("Idle");
        [HideInInspector] public int RunHash = Animator.StringToHash("Run");
        [HideInInspector] public int RunGunHash = Animator.StringToHash("RunGun");
        [HideInInspector] public int IdleAimingHash = Animator.StringToHash("IdleAiming");
    }
}