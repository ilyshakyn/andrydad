using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightToggleTarget : MonoBehaviour
{
    [SerializeField] protected bool initiallyActive = true;


    public virtual void Awake()
    {
        if (gameObject != null)
            gameObject.SetActive(initiallyActive);
    }

   
    public virtual void EnableObject()
    {
        if (gameObject != null)
            gameObject.SetActive(true);
    }

    
    public virtual void DisableObject()
    {
        if (gameObject != null)
            gameObject.SetActive(false);
    }
}
