using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFolow : MonoBehaviour
{
    InputMovePlayer inputMovePlayer;

    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;
    public LayerMask collisionLayers;
    private float defoultPosition;
    private Vector3 cameraFolowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float CameraCollisionOffset = 0.2f;
    public float minimumCollisonOffset = 0.6f;
    public float cameraCollisionRadius = 2;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle;
    public float pivotAngle;
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = +35;

    private void Awake()
    {
        inputMovePlayer = FindObjectOfType<InputMovePlayer>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defoultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FolowTarget();
        RotateCamera();
        HandleCameraCollision();
    }

    public void FolowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFolowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    public void RotateCamera()
    {
        Vector3 rotation;
        lookAngle = lookAngle + (inputMovePlayer.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputMovePlayer.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollision()
    {
        float targetPosition = defoultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        // Убираем лишнюю точку с запятой
        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = Mathf.Clamp(-(distance - CameraCollisionOffset), -Mathf.Abs(defoultPosition), 0);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisonOffset)
        {
            targetPosition = targetPosition - minimumCollisonOffset;
        }

        cameraVectorPosition = cameraTransform.localPosition;
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
