using System.Collections;
using Agava.YandexGames;
using Plugins.MonoCache;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reflex
{
    public class Loader : MonoCache
    {
        private IEnumerator Start()
        {
#if UNITY_EDITOR
            Debug.Log("Imitation SDK initialized");
            LaunchGame();
            yield break;
#endif
            yield return YandexGamesSdk.Initialize(LaunchGame);
            Debug.Log("SDK initialized");
        }

        private void LaunchGame()
        {
            Scene scene = SceneManager.LoadScene("Main", new LoadSceneParameters(LoadSceneMode.Single));
            ReflexSceneManager.PreInstallScene(scene, builder => builder.AddSingleton(""));
        }
    }
}