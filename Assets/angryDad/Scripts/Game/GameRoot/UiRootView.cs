using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiRootView : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform uiScreenContainer;

    private void Awake()
    {
        HideLoadingScreen();
    }
    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }


    private void AttacheSceneUI(GameObject sceneUI)
    {
        ClearSceneUI();
        sceneUI.transform.SetParent(uiScreenContainer, false);
    }

    private void ClearSceneUI()
    {
        int cildCount = uiScreenContainer.childCount;

        for (int i = 0; i < cildCount; i++)
        {
            Destroy(uiScreenContainer.GetChild(i).gameObject);
        }

    }

}
