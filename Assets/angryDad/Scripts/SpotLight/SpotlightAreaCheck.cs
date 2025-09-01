using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightAreaCheck : MonoBehaviour
{
    public string targetTag = "Player";
    private Transform target;
    private Light spotLight;

    void Start()
    {
        spotLight = GetComponent<Light>();

        GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObj != null)
            target = targetObj.transform;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;
    }
}