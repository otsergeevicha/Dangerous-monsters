﻿using System.Linq;
using Assistant.AI.Parents;
using BehaviorDesigner.Runtime.Tasks;
using Turrets.Childrens;

namespace Assistant.AI
{
    public class MoveCartridgeGun : CargoAssistantAction
    {
        private CartridgeGun _gun;
        
        public override void OnStart()
        {
            _gun = CargoAssistant.CartridgeGuns.FirstOrDefault(cartridge => cartridge.IsRequiredDownload);

            if (_gun != null)
            {
                CargoAssistant.AssistantAnimation.EnableRun();
                Agent.speed = CargoAssistant.AssistantData.Speed;
                Agent.SetDestination(_gun.transform.position);
            }
        }

        public override TaskStatus OnUpdate() => 
            _gun.IsRequiredDownload ? TaskStatus.Success : TaskStatus.Failure;
    }
}