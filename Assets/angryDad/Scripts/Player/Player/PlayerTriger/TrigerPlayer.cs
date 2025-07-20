using Assets.angryDad.Scripts.Trigers.LogicTrigers.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerPlayer : MonoBehaviour, ITriggerDamageReceiver
{

    private PlayerTrigerHp _health;

    private void Awake()
    {
        _health = GetComponent<PlayerTrigerHp>();
    }

    public void ApplyTriggerDamage(float amount)
    {
        _health.triggerHpPlayer -= amount;
    }
}
