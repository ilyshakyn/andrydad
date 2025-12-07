    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.angryDad.Scripts.Entry
{
    public class GameEntryPoint: MonoBehaviour
    {
        private static GameEntryPoint instance;
        private Courootins courootins;
        private UiRootView uiRootView;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoStartGame()
        {
            //системные настройки

            instance = new GameEntryPoint();
            instance.RunGame();
        }

        public GameEntryPoint()
        {
            courootins = new GameObject("[COROUTINES]").AddComponent<Courootins>();
            UnityEngine.Object.DontDestroyOnLoad(courootins.gameObject);


            var prefabUiRoot = Resources.Load<UiRootView>("UIRoot");
            uiRootView = UnityEngine.Object.Instantiate(prefabUiRoot);
            UnityEngine.Object.DontDestroyOnLoad(uiRootView.gameObject);

            //создание контейнера
            //тут могут подгружатьс янастройки графона измен игроком или назначеные клавиши     

        } 
        private void RunGame()
        {
#if UNITY_EDITOR    
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName== ScenesName.level0)
            {
               courootins.StartCoroutine(LoadAndStartLevel());
                return;
            }
            if (sceneName == ScenesName.mainMenu)
            {
                courootins.StartCoroutine(LoadAndStartMainMenu());
                return;
            }
            if (sceneName != ScenesName.boot)
            {
                return;
            }
#endif

            courootins.StartCoroutine(LoadAndStartLevel());
        }

        private IEnumerator LoadAndStartLevel()
        {
            uiRootView.ShowLoadingScreen();

            yield return LoadScene(ScenesName.boot);
            yield return LoadScene(ScenesName.level0);

            yield return new WaitForSeconds(5f);

            var sceneEntryPoint = UnityEngine.Object.FindObjectOfType<GamePlayEntryPoint>();
            sceneEntryPoint.Run(uiRootView);

            sceneEntryPoint.GoTOMainMenuSceneRequested += () =>
            {
                courootins.StartCoroutine(LoadAndStartMainMenu());
            };

            uiRootView.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartMainMenu()
        {
            uiRootView.ShowLoadingScreen();

            yield return LoadScene(ScenesName.boot);
            yield return LoadScene(ScenesName.mainMenu);

            yield return new WaitForSeconds(5f);

            var sceneEntryPoint = UnityEngine.Object.FindObjectOfType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(uiRootView);


            sceneEntryPoint.GoTOGameplaySceneRequested += () =>
            {
                courootins.StartCoroutine(LoadAndStartLevel());
            };

            uiRootView.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string scenemame)
        {
            yield return SceneManager.LoadSceneAsync(scenemame);
        }
    }
}
