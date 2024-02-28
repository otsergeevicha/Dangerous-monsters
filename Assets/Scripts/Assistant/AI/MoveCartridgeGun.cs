using System.Linq;
using Assistant.AI.Parents;
using Turret;

namespace Assistant.AI
{
    public class MoveCartridgeGun : CargoAssistantAction
    {
        public override void OnStart()
        {
            CartridgeGun gun = CargoAssistant.CartridgeGuns.FirstOrDefault(cartridge => cartridge.IsRequiredDownload);

            if (gun != null)
            {
                CargoAssistant.AnimationOperator.EnableRun();
                Agent.speed = CargoAssistant.AssistantData.Speed;
                Agent.SetDestination(gun.transform.position);
            }
        }
    }
}