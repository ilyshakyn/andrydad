using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadMove : MonoBehaviour
{
    [SerializeField] private InputActionReference cameraLookAction; // ? Привязываем Camera (Delta Mouse)

    [SerializeField] private float verticalSpeed = 1.0f;
    [SerializeField] private float minY = 1.2f;
    [SerializeField] private float maxY = 2.0f;
    [SerializeField] private float smoothSpeed = 5.0f;

    private float currentY;
    private Vector3 targetPosition;

    private void Start()
    {
        currentY = transform.position.y;
        targetPosition = transform.position;

        // Активируем action (важно!)
        cameraLookAction.action.Enable();
    }

    private void Update()
    {
        Vector2 mouseDelta = cameraLookAction.action.ReadValue<Vector2>();
        float mouseY = mouseDelta.y;

        currentY += mouseY * verticalSpeed * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        Vector3 pos = transform.position;
        targetPosition = new Vector3(pos.x, currentY, pos.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}