using System;
using System.Collections;
using UnityEngine;


    public class UIMainMenuRootBinder : MonoBehaviour
    {

       public event Action goToGameplayButtonClick;

       public void HandleGoToGameplayButtonClick()
       {
         goToGameplayButtonClick?.Invoke();
       }

    }
