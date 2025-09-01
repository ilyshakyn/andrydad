using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigerHp : MonoBehaviour
{
    public float triggerHpPlayer = 100f;


    private void Update()
    {
        if(triggerHpPlayer<=0)
            Destroy(gameObject);
    }
}
