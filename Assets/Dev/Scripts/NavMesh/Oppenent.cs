using UnityEngine.AI;

public class Oppenent : PoolableObject
{
    public AiScript movement;
    public NavMeshAgent agent;

    public virtual void OnEnable()
    {
       

    }


    public override void OnDisable()
    {
        base.OnDisable();
        agent.enabled = false;
    }

   

}
