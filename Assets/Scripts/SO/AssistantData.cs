using ContactPlatforms;
using Turret;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewAssistant", menuName = "Characters/Assistant", order = 1)]
    public class AssistantData : ScriptableObject
    {
        [SerializeField] private CartridgeGun[] _cartridgeGuns;
        [SerializeField] private StorageAmmoPlate _storageAmmoPlate;
        
        [HideInInspector] public int AssistantIdleHash = Animator.StringToHash("Idle");
    }
}