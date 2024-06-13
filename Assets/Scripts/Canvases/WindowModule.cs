using Infrastructure.Factory.Pools;
using Services.Bank;
using SO;

namespace Canvases
{
    public class WindowModule
    {
        private IWallet _wallet;
        private Hud _hud;

        public void Construct(StoreAssistantPlate storeAssistantPlate,
            StoreTurretPlate[] storeTurretPlates, PoolData poolData,
            Pool pool, IWallet wallet, Hud hud)
        {
            _wallet = wallet;
            _hud = hud;
            
            _wallet.MoneyChanged += _hud.UpdateMoneyView;
            _wallet.GemChanged += _hud.UpdateGemView;
            
            _hud.UpdateMoneyView(_wallet.ReadCurrentMoney());
            _hud.UpdateGemView(_wallet.ReadCurrentGem());
            
            storeAssistantPlate.Construct(poolData.MaxCountCargoAssistant, pool.PoolCargoAssistant);

            for (int i = 0; i < storeTurretPlates.Length; i++) 
                storeTurretPlates[i].Construct(pool.PoolTurrets);
        }

        public void Dispose()
        {
            _wallet.MoneyChanged -= _hud.UpdateMoneyView;
        }
    }
}