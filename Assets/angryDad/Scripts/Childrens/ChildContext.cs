using Assets.angryDad.Scripts.Player.ScareLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildContext : MonoBehaviour
{
    public ChildController Controller { get; }
    public IChildAnimator Animator { get; }
    public IChildMover Mover { get; }
    public Transform Hero { get; }
    public Transform Playground { get; }
    public MicrophoneScareService ScareService { get; }
    public ChildContext(ChildController controller, IChildAnimator animator, IChildMover mover, Transform hero, Transform playground, MicrophoneScareService scareService)
    {
        Controller = controller;
        Animator = animator;
        Mover = mover;
        Hero = hero;
        Playground = playground;
        ScareService = scareService;
    }
}
