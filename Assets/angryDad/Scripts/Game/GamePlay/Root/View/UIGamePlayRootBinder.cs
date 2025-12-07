using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGamePlayRootBinder : MonoBehaviour
{
    public event Action goToMainMenuButtonClick;


    public void HandleGoToMainMenuButtonClick()
    {
        goToMainMenuButtonClick?.Invoke();
    }
}
