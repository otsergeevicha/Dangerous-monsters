using System.Collections.Generic;
using Services.Factory;
using SO;
using Turrets;

namespace Infrastructure.Factory.Pools
{
    public class PoolTurrets
    {
        public List<Turret> Turrets { get; private set; } = new();
        
        public PoolTurrets(IGameFactory factory, TurretPlate[] turretPlates, TurretData turretData,
            PoolMissiles poolMissiles)
        {
            int maxCount = turretPlates.Length;

            for (int i = 0; i < maxCount; i++)
            {
                Turret turret = factory.CreateTurret();
                turret.Construct(turretPlates[i].GetCartridgeGun, turretData, poolMissiles);
                turret.InActive();
                Turrets.Add(turret);
            }
        }
    }
}