using Infrastructure.Assets;
using Infrastructure.Factory;
using Inputs;
using Plugins.MonoCache;
using Reflex.Core;
using Services.Factory;
using Services.Inputs;

namespace Reflex
{
    public class ProjectInstaller : MonoCache, IInstaller
    {
        public void InstallBindings(ContainerBuilder descriptor)
        {
            InputService inputService = new InputService();
            GameFactory gameFactory = new GameFactory(new AssetsProvider());
            
             descriptor.AddSingleton(inputService, typeof(IInputService));
             descriptor.AddSingleton(gameFactory, typeof(IGameFactory));
        }
    }
}