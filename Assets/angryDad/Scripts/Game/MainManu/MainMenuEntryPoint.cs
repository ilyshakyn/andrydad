using System;
using System.Collections;
using UnityEngine;


    public class MainMenuEntryPoint : MonoBehaviour
    {
        public event Action GoTOGameplaySceneRequested;

        [SerializeField] private UIMainMenuRootBinder sceneUIRootPrefab;


        public void Run(UiRootView uiRoot)
        {
            var uiScene = Instantiate(sceneUIRootPrefab);
            uiRoot.AttacheSceneUI(uiScene.gameObject);

            uiScene.goToGameplayButtonClick += () =>
            {
                GoTOGameplaySceneRequested?.Invoke();
            };
        }
    
    }