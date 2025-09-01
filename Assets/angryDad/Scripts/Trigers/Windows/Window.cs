using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Window : BaseSourceTrigger
{
    [SerializeField] private Light windowsLight;



    protected override void Start()
    {
        windowsLight = GetComponentInChildren<Light>();
        base.Start();
        
    }

    public override void TurnOn()
    {


        windowsLight.enabled = true;
        
        base.TurnOn();
        Debug.Log($"{name}: Turned ON");
    }


    public override void TurnOff()
    {
       
        windowsLight.enabled = false;
        
        base.TurnOff();
        
    }


}
