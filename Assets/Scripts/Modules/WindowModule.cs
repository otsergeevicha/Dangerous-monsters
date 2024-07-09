using Canvases;
using Infrastructure.Factory.Pools;
using Services.Bank;
using Services.Inputs;
using SO;

namespace Modules
{
    public class WindowModule
    {
        private IWallet _wallet;
        private Hud _hud;
        private LoseScreen _loseScreen;
        private IInputService _input;
        private StartScreen _startScreen;
        private WinScreen _winScreen;

        public void Construct(StoreAssistantPlate storeAssistantPlate,
            StoreTurretPlate[] storeTurretPlates, PoolData poolData,
            Pool pool, IWallet wallet, Hud hud, LoseScreen loseScreen, StartScreen startScreen, WinScreen winScreen,
            IInputService input)
        {
            _winScreen = winScreen;
            _input = input;
            _startScreen = startScreen;
            _loseScreen = loseScreen;
            _wallet = wallet;
            _hud = hud;

            _wallet.MoneyChanged += _hud.UpdateMoneyView;
            _wallet.GemChanged += _hud.UpdateGemView;

            _startScreen.OnClickStart += LaunchGame;
            _loseScreen.OnClickReStart += TryAgain;

            _hud.UpdateMoneyView(_wallet.ReadCurrentMoney());
            _hud.UpdateGemView(_wallet.ReadCurrentGem());
            
            storeAssistantPlate.Construct(poolData.MaxCountCargoAssistant, pool.PoolCargoAssistant);

            for (int i = 0; i < storeTurretPlates.Length; i++)
                storeTurretPlates[i].Construct(pool.PoolTurrets);

            _input.OffControls();
            startScreen.OnActive();
        }

        public void Dispose()
        {
            _wallet.MoneyChanged -= _hud.UpdateMoneyView;
            _wallet.GemChanged -= _hud.UpdateGemView;
            
            _startScreen.OnClickStart -= LaunchGame;
            _loseScreen.OnClickReStart -= TryAgain;
        }

        public void HeroDied()
        {
            _input.OffControls();
            _loseScreen.OnActive();
        }

        private void LaunchGame() => 
            _input.OnControls();

        private void TryAgain()
        {
            _startScreen.OnActive();
        }

        public void WinScreen()
        {
            _input.OffControls();
            _winScreen.OnActive();
        }

        public void UpLevelCompleted() => 
            _winScreen.ActiveButtonContinue();
    }
}