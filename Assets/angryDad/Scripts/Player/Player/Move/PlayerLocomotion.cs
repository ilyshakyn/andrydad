using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputMovePlayer inputMovePlayer;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigibody;

    public bool isAttake;
    public bool isTurn;
    public bool isRunning;
    public float gravityMultiplier = 2f;

    [Header("Movement Speed")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 7;
    public float rotationSpeed = 15;



    private void Awake()
    {
        inputMovePlayer = GetComponent<InputMovePlayer>();
        playerRigibody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandlMovement();
        HandleRotation();
      
    }

    private void HandlMovement()
    {
        moveDirection = cameraObject.forward * inputMovePlayer.verticalInput;
        moveDirection += cameraObject.right * inputMovePlayer.horizontalInput;
        moveDirection.Normalize();

        if (isRunning)
            moveDirection = moveDirection * runningSpeed;
        else
            moveDirection = moveDirection * walkingSpeed;

        Vector3 movementVelocity = moveDirection;
        movementVelocity.y = playerRigibody.velocity.y;

        if (movementVelocity.y < 0)
            movementVelocity.y *= gravityMultiplier;

        playerRigibody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputMovePlayer.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputMovePlayer.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
    }

    
}