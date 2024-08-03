using Canvases;
using Canvases.UpgradePlayer;
using Infrastructure.Factory.Pools;
using Player;
using Services.Bank;
using Services.Inputs;
using Services.SDK;
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
        private UpgradePlayerBoard _upgradePlayerBoard;
        private UpgradeHeroScreen _upgradeHeroScreen;
        private Hero _hero;

        public void Construct(StoreAssistantPlate storeAssistantPlate,
            StoreTurretPlate[] storeTurretPlates, PoolData poolData,
            Pool pool, IWallet wallet, Hud hud, LoseScreen loseScreen, StartScreen startScreen, WinScreen winScreen,
            IInputService input, UpgradePlayerBoard upgradePlayerBoard, UpgradeHeroScreen upgradeHeroScreen,
            HeroData heroData, PriceListData priceList, Hero hero, ISDKService sdk)
        {
            _hero = hero;
            _upgradeHeroScreen = upgradeHeroScreen;
            _upgradePlayerBoard = upgradePlayerBoard;
            _winScreen = winScreen;
            _input = input;
            _startScreen = startScreen;
            _loseScreen = loseScreen;
            _wallet = wallet;
            _hud = hud;

            _wallet.MoneyChanged += _hud.UpdateMoneyView;
            _wallet.GemChanged += _hud.UpdateGemView;

            _startScreen.OnClickStart += LaunchGame;
            _loseScreen.OnClickReStart += _hero.TryAgain;

            _upgradePlayerBoard.OnEntered += OnHeroUpgrade;
            _wallet.MoneyChanged += _upgradeHeroScreen.UpdateMoneyView;

            _hud.OnGameOver += HeroDied;

            _hud.UpdateMoneyView(_wallet.ReadCurrentMoney());
            _hud.UpdateGemView(_wallet.ReadCurrentGem());

            _winScreen.Construct(_wallet, sdk);
            _loseScreen.Construct(wallet, sdk, priceList);
            
            storeAssistantPlate.Construct(poolData.MaxCountCargoAssistant, pool.PoolCargoAssistant);

            for (int i = 0; i < storeTurretPlates.Length; i++)
                storeTurretPlates[i].Construct(pool.PoolTurrets, _wallet, priceList, sdk);

            upgradeHeroScreen.Construct(_input, heroData, priceList, _wallet, hero, sdk);
            
            _input.OffControls();
            startScreen.OnActive();
        }

        public void Dispose()
        {
            _wallet.MoneyChanged -= _hud.UpdateMoneyView;
            _wallet.GemChanged -= _hud.UpdateGemView;
            
            _startScreen.OnClickStart -= LaunchGame;
            _loseScreen.OnClickReStart -= _hero.TryAgain;
            
            _upgradePlayerBoard.OnEntered -= OnHeroUpgrade;
            _wallet.MoneyChanged -= _upgradeHeroScreen.UpdateMoneyView;
            
            _hud.OnGameOver -= HeroDied;
        }

        public void HeroDied()
        {
            _input.OffControls();
            _loseScreen.OnActive();
        }

        private void OnHeroUpgrade() => 
            _upgradeHeroScreen.OnActive();

        private void LaunchGame() => 
            _input.OnControls();

        public void WinScreen()
        {
            _input.OffControls();
            _winScreen.OnActive();
            _hud.UpdateMonstersCounter();
        }

        public void UpLevelCompleted() => 
            _winScreen.ActiveButtonContinue();
    }
}