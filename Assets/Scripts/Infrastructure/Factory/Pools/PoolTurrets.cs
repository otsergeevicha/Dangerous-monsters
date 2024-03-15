using System.Collections.Generic;
using Services.Factory;
using Turrets;

namespace Infrastructure.Factory.Pools
{
    public class PoolTurrets
    {
        public List<Turret> Turrets { get; private set; } = new();
        
        public PoolTurrets(IGameFactory factory, TurretPlate[] turretPlates)
        {
            int maxCount = turretPlates.Length;

            for (int i = 0; i < maxCount; i++)
            {
                Turret turret = factory.CreateTurret();
                turret.Construct(turretPlates[i].GetCartridgeGun);
                turret.InActive();
                Turrets.Add(turret);
            }
        }
    }
}