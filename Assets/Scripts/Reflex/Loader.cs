using Plugins.MonoCache;
using Reflex.Core;
using UnityEngine.SceneManagement;

namespace Reflex
{
    public class Loader : MonoCache
    {
        private void Start() =>
            LaunchGame();

        private void LaunchGame()
        {
            Scene scene = SceneManager.LoadScene("Main", new LoadSceneParameters(LoadSceneMode.Single));
            ReflexSceneManager.PreInstallScene(scene, builder => builder.AddSingleton(""));
        }
    }
}