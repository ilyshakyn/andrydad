using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class InputMovePlayer : MonoBehaviour
{

     PlayerControls playerControls;
     PlayerLocomotion playerLocomotion;
     PlayerAnimator playerAnimator;

     public Vector2 movementInput;
     public Vector2 cameraInput;

     public float cameraInputX;
     public float cameraInputY;


     public float moveAmount;
     public float verticalInput;
     public float horizontalInput;

     public bool b_Input;
     public bool lKM_Input;
     public bool f_input;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {

        if (playerControls == null)
        {

            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;

            playerControls.PlayerActions.Attake.performed += i => lKM_Input = true;
            playerControls.PlayerActions.Attake.canceled += i => lKM_Input = false;


            playerControls.PlayerActions.Turn.performed += i => f_input = true;
            playerControls.PlayerActions.Turn.canceled += i => f_input = false;
        }

        playerControls.Enable();

    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleRunningInput();
        HandleAttakeInput();
        HandleTurnInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
       
        playerAnimator.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isRunning);
    }

    private void HandleAttakeInput() 
    {
      
        if (lKM_Input)
        {
            playerLocomotion.isAttake = true;
        }
        else
        {
            playerLocomotion.isAttake = false;
        }

        playerAnimator.Attake(playerLocomotion.isAttake);
    }

    private void HandleTurnInput()
    {

        if (f_input)
        {
            playerLocomotion.isTurn = true;
        }
        else
        {
            playerLocomotion.isTurn = false;
        }

        playerAnimator.IsTurn(playerLocomotion.isTurn);
    }

    private void HandleRunningInput()
    {
        if (b_Input && moveAmount > 0.55f)
        {
            playerLocomotion.isRunning = true;
        }
        else
        {
            playerLocomotion.isRunning = false;
        }
    }

}
