using System.Collections;
using Agava.YandexGames;
using Plugins.MonoCache;
using Reflex.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reflex
{
    public class Loader : MonoCache
    {
        [SerializeField] private TMP_Text _nameGame;
        
        private const string EngName = "Dangerous monsters";
        private const string RuName = "Опасные монстры";
        
        private IEnumerator Start()
        {
#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
            
            Debug.Log("Imitation SDK initialized");
            LaunchGame();
            yield break;
#endif
            yield return YandexGamesSdk.Initialize(LaunchGame);
            Debug.Log("SDK initialized");
            
            UpdateNameGame();
            
            if (PlayerAccount.IsAuthorized)
                PlayerAccount.GetCloudSaveData(OnSuccessCallback, OnErrorCallback);
            else
                LaunchGame();
        }
        
        private void UpdateNameGame() =>
            _nameGame.text = YandexGamesSdk.Environment.i18n.lang == "en"
                ? EngName
                : RuName;

        private void OnSuccessCallback(string data)
        {
            PlayerPrefs.SetString(Constants.Progress, data);
            PlayerPrefs.Save();
            
            LaunchGame();
        }

        private void OnErrorCallback(string _) => 
            LaunchGame();

        private void LaunchGame()
        {
            Scene scene = SceneManager.LoadScene("Main", new LoadSceneParameters(LoadSceneMode.Single));
            ReflexSceneManager.PreInstallScene(scene, builder => builder.AddSingleton(""));
        }
    }
}