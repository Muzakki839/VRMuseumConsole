using UnityEngine;
using UnityEngine.AI;

public class BasicNavMesh : MonoBehaviour
{
    public Transform wanderCenter;
    public float wanderRadius = 10f;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = waitTime;
        GoToRandomPoint();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                GoToRandomPoint();
                timer = waitTime;
            }
        }
    }

    void GoToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * (wanderRadius * Mathf.Pow(Random.value, 2));

        randomDirection += wanderCenter.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
