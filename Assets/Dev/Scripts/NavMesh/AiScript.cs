using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Rendering;

[RequireComponent(typeof(NavMeshAgent),typeof(AgentLinkMover))]

public class AiScript : MonoBehaviour
{
    //public Camera cam;
    [SerializeField] private Animator animator;

    

    public Transform followTarget;
    public float updateSpeed = 0.1f; // how frequently to recalculate payh based on Target transform's position
    private NavMeshAgent agent;


    private AgentLinkMover linkMover;


    private const string IS_WALKING = "IsWalking";
    private const string JUMP = "Jump";
    private const string ON_AIR = "OnAir";
    private const string LANDED = "Landed";

    private Coroutine FollowCoroutine;

   

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        linkMover = GetComponent<AgentLinkMover>();
        linkMover.OnLinkStart += HandleLinkStart;
        linkMover.OnLinkUpdate += HandleLinkUpdate;
        linkMover.OnLinkEnd += HandleLinkEnd;

    }

    //Jump Phases
    private void HandleLinkStart()
    {
        animator.SetTrigger(JUMP);
    }
    private void HandleLinkUpdate()
    {
        animator.SetTrigger(ON_AIR);
        
    }
    private void HandleLinkEnd()
    {
        animator.SetTrigger(LANDED);
    }

    private void Start()
    {
       
    }
    private void Update()
    {
        animator.SetBool(IS_WALKING, agent.velocity.magnitude > 0.01f);
    }

    public void StartChasing()
    {
        if(FollowCoroutine == null)
        {
           FollowCoroutine = StartCoroutine(FollowTarget());

        }
        else
        {
            Debug.LogWarning("Called StartChasing on oppenent that is already chasing! this is likely a bug in some calling class!");
        }

    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(updateSpeed);
        
        while (enabled) 
        {
           
            agent.SetDestination(followTarget.transform.position);
            yield return Wait;
        
        
        }
    }

 

}
