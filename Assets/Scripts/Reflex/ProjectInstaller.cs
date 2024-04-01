using Bank;
using Infrastructure.Assets;
using Infrastructure.Factory;
using Inputs;
using Plugins.MonoCache;
using Reflex.Core;
using SaveLoadModule;
using Services.Bank;
using Services.Factory;
using Services.Inputs;
using Services.SaveLoad;

namespace Reflex
{
    public class ProjectInstaller : MonoCache, IInstaller
    {
        public void InstallBindings(ContainerBuilder descriptor)
        {
            InputService inputService = new InputService();
            GameFactory gameFactory = new GameFactory(new AssetsProvider());
            SaveLoad saveLoad = new SaveLoad();
            Wallet wallet = new Wallet(saveLoad);

            descriptor.AddSingleton(inputService, typeof(IInputService));
            descriptor.AddSingleton(gameFactory, typeof(IGameFactory));
            descriptor.AddSingleton(saveLoad, typeof(ISave));
            descriptor.AddSingleton(wallet, typeof(IWallet));
        }
    }
}