using Assets.angryDad.Scripts.Childrens.Animation;
using Assets.angryDad.Scripts.Childrens.Factory;
using Assets.angryDad.Scripts.Childrens.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class ChildController : MonoBehaviour
{
    private ChildStateMachine stateMachine;
    private IChildAnimator animator;
    private IChildMover mover;

    [SerializeField] private Transform heroTarget;
    [SerializeField] private Animator animatorOnModel;

    private void Awake()
    {
        stateMachine = new ChildStateMachine();

        var navMeshAgent = GetComponent<NavMeshAgent>();
        animator = new ChildAnimator(animatorOnModel);
        mover = new NavMeshMover(navMeshAgent);
    }

    private void Start()
    {
        if (heroTarget == null)
        {
            var heroObj = GameObject.FindGameObjectWithTag("Hero");
            if (heroObj != null)
                heroTarget = heroObj.transform;
            else
                Debug.LogError("Hero not found. Please assign 'Hero' tag to the player.");
        }

        var context = new ChildContext(this, animator, mover, heroTarget);
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

}
