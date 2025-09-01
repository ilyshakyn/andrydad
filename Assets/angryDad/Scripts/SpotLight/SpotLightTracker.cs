using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightTracker : MonoBehaviour
{
    [Tooltip("Тег целей, на которые будет светить Spot Light.")]
    public string targetTag = "Target";

    [Tooltip("Скорость поворота света к цели.")]
    public float rotationSpeed = 5f;

    [Tooltip("Как часто менять цель (в секундах).")]
    public float changeTargetInterval = 2f;

    private Transform target;
    private Light spotLight;
    public Light spotLightChild;
    public CapsuleCollider capsuleCollider;

    private List<Transform> allTargets = new List<Transform>();
    private float timer;

    void Start()
    {
        // Получаем сам светильник (Spot Light)
        spotLight = GetComponent<Light>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();

        if (spotLight == null || spotLight.type != LightType.Spot)
        {
            Debug.LogError("Этот скрипт должен висеть на объекте с Light (Spot).");
            enabled = false;
            return;
        }

        // Находим все объекты с указанным тегом
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        if (targets.Length > 0)
        {
            foreach (GameObject obj in targets)
            {
                allTargets.Add(obj.transform);
            }
            ChooseRandomTarget();
        }
        else
        {
            Debug.LogError("Не найдено ни одного объекта с тегом '" + targetTag + "'.");
            enabled = false;
        }
        if (target == null) return;

        // Таймер для смены цели
        timer += Time.deltaTime;
        if (timer >= changeTargetInterval)
        {
            ChooseRandomTarget();
            timer = 0f;
        }

        // Вычисляем направление до цели
        Vector3 directionToTarget = target.position - transform.position;

        // Плавно поворачиваем Spot Light к цели
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        spotLightChild.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Настраиваем дальность света
        float distance = Vector3.Distance(transform.position, target.position);
        spotLight.range = distance;
        spotLightChild.range = distance;
        capsuleCollider.transform.position = transform.position + transform.forward * spotLight.range;
    }

    void Update()
    {
        
    }

    private void ChooseRandomTarget()
    {
        if (allTargets.Count == 0) return;
        target = allTargets[Random.Range(0, allTargets.Count)];
    }
}