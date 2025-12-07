using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayEntryPoint : MonoBehaviour
{

    public event Action GoTOMainMenuSceneRequested;

    [SerializeField] private UIGamePlayRootBinder sceneUIRootPrefab;


    public void Run(UiRootView uiRoot)
    {
        var uiScene = Instantiate(sceneUIRootPrefab);
        uiRoot.AttacheSceneUI(uiScene.gameObject);

        uiScene.goToMainMenuButtonClick += () =>
        {
            GoTOMainMenuSceneRequested?.Invoke();
        };
    }


}
