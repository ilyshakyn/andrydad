using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputMovePlayer inputMovePlayer;
    PlayerLocomotion playerLocomotion;
    CameraFolow cameraFolow;

    private void Awake()
    {
        inputMovePlayer = GetComponent<InputMovePlayer>(); 
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraFolow = FindObjectOfType<CameraFolow>();
    }

    private void Update()
    {
        inputMovePlayer.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }


    private void LateUpdate()
    {
        cameraFolow.HandleAllCameraMovement();
    }
}
