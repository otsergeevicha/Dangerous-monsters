using System;
using ContactPlatforms;
using Plugins.MonoCache;
using SO;
using Turret;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AnimationOperator))]
    public class CargoAssistant : MonoCache
    {
        public AnimationOperator AnimationOperator{ get; private set; }
        public AssistantData AssistantData { get; private set; }
        public StorageAmmoPlate StorageAmmoPlate { get; private set; }
        public CartridgeGun[] CartridgeGuns { get; private set; }
        public bool IsEmpty { get; private set; } = true;
        public bool IsFulled { get; private set; } = false;

        public void Construct(AssistantData assistantData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate)
        {
            AssistantData = assistantData;
            StorageAmmoPlate = storageAmmoPlate;
            CartridgeGuns = cartridgeGuns;

            AnimationOperator = Get<AnimationOperator>();
            AnimationOperator.Construct(assistantData);
        }

        public void InActive()
        {
            gameObject.SetActive(false);
        }
    }
}