using Assets.angryDad.Scripts.Childrens.Animation;
using Assets.angryDad.Scripts.Childrens.Factory;
using Assets.angryDad.Scripts.Childrens.StateMachine;
using Assets.angryDad.Scripts.Player.ScareLogic;
using Assets.angryDad.Scripts.Systems;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Rendering.LookDev;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class ChildController : MonoBehaviour
{


    private ChildStateMachine stateMachine;
    private IChildAnimator animator;
    private IChildMover mover;
    private MicrophoneScareService scareService;

    [SerializeField] private Transform heroTarget;
    [SerializeField] private Transform playgroundPoint;
    [SerializeField] private Animator animatorOnModel;


    private void Awake()
    {
        stateMachine = new ChildStateMachine();

        var navMeshAgent = GetComponent<NavMeshAgent>();
        animator = new ChildAnimator(animatorOnModel);
        mover = new NavMeshMover(navMeshAgent);

        scareService = ServiceLocator.Resolve<MicrophoneScareService>();
    }
    
    private void Start()
    {
        if (heroTarget == null  || playgroundPoint == null)
        {
            var heroObj = GameObject.FindGameObjectWithTag("Hero");
            if (heroObj != null)
                heroTarget = heroObj.transform;
            else
                Debug.LogError("Hero not found. Please assign 'Hero' tag to the player.");

            var PlayGround = GameObject.FindGameObjectWithTag("PlayGround");
            if (PlayGround != null)
                playgroundPoint = PlayGround.transform;
            else
                Debug.LogError("Hero not found. Please assign 'Hero' tag to the player.");

        }


        var context = new ChildContext(this, animator, mover, heroTarget, playgroundPoint, scareService);
        var initialState = ChildStateFactory.CreateState(ChildStateType.Play, context);
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeStateExternally(IChildState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public ChildContext GetContext()
    {
        return new ChildContext(this, animator, mover, heroTarget, playgroundPoint, scareService);
    }

    private void OnEnable()
    {
        GameEvents.OnScareByMic += HandleMicScare;
        GameEvents.OnRunBackHomeAfterDelay += HandleReturnToHouse;
    }

    private void OnDisable()
    {
        GameEvents.OnScareByMic -= HandleMicScare;
        GameEvents.OnRunBackHomeAfterDelay -= HandleReturnToHouse;
    }

    private void HandleMicScare(ChildController child)
    {
        if (child == this)
        {
            Debug.Log("fdsfsdfsdfsdfsdfsdfdsfsdfsfsdf");
            ChangeStateExternally(new RunAwayState(GetContext()));
        }
    }

    private void HandleReturnToHouse(ChildController child)
    {
        if (child == this)
        {
            ChangeStateExternally(ChildStateFactory.CreateState(ChildStateType.WalkToHouse, GetContext()));
        }
    }



}
