using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(NavMeshAgent))]
public class AiNavMeshHandler : MonoBehaviour
{
    private NavMeshAgent Agent;
    public float RotationSpeed = 5f;
    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 Destination)
    {
        Agent.isStopped = false;
        Agent.SetDestination(Destination);
    }
    public void StopPathFinding()
    {
        Agent.isStopped=true;
    }
    public void RotateTowards(Transform Target)
    {
        Vector3 Direction = (Target.position - transform.position).normalized;
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(Direction.x, 0, Direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * RotationSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
