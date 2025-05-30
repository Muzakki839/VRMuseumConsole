using UnityEngine;
using UnityEngine.AI;

public class NavMeshWander : MonoBehaviour
{
    public Transform wanderCenter;
    public float wanderRadius = 10f;
    public float waitTime = 2f;

    public float interactionDistance = 2f;
    public float avoidPlayerDistance = 5f;
    public float detectEasterEggRadius = 4f;

    [Range(0f, 1f)] public float clapChance = 0.03f;
    public float eggReactDuration = 3f;
    public float eggCooldown = 10f;

    public float talkDuration = 3f;
    public float talkCooldown = 10f;

    private NavMeshAgent agent;
    private Animator anim;
    private float timer;
    private Vector3 currentWanderTarget;

    enum AIState { Wander, AvoidPlayer, TalkNPC, ReactToEasterEgg, Idle }
    private AIState currentState = AIState.Wander;

    // Cooldown trackers
    private float lastEggTime = -Mathf.Infinity;
    private float lastTalkTime = -Mathf.Infinity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        timer = waitTime;
        PickNewWanderTarget();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Wander:
                WanderUpdate();
                DetectNearby();
                break;
            case AIState.AvoidPlayer:
                AvoidPlayerUpdate();
                break;
            case AIState.TalkNPC:
                // While talking, face NPC
                FaceClosestNPC();
                break;
            case AIState.ReactToEasterEgg:
                ReactToEasterEggUpdate();
                break;
            case AIState.Idle:
                // no action; waiting to exit
                break;
        }
        Animate();
    }

    void WanderUpdate()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                PickNewWanderTarget();
                timer = waitTime;
            }
        }
    }

    void DetectNearby()
    {
        // Priority: Avoid player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player && Vector3.Distance(transform.position, player.transform.position) < avoidPlayerDistance)
        {
            EnterState(AIState.AvoidPlayer);
            return;
        }

        // Talk to NPC
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject && hit.GetComponent<NavMeshWander>())
            {
                if (Time.time - lastTalkTime >= talkCooldown)
                {
                    lastTalkTime = Time.time;
                    // Stop and talk
                    EnterState(AIState.TalkNPC, talkDuration);
                    anim.SetBool("isTalking", true);
                    return;
                }
            }
        }

        // React to Easter Egg (clap)
        Collider[] eggs = Physics.OverlapSphere(transform.position, detectEasterEggRadius);
        foreach (var egg in eggs)
        {
            if (egg.CompareTag("EasterEgg") && Time.time - lastEggTime >= eggCooldown)
            {
                lastEggTime = Time.time;
                // Stop and clap
                EnterState(AIState.ReactToEasterEgg, eggReactDuration);
                anim.SetBool("isClapping", true);
                return;
            }
        }
    }

    void AvoidPlayerUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Vector3 away = (transform.position - player.transform.position).normalized;
            // Bias towards original wander target
            Vector3 biasedTarget = currentWanderTarget + away * interactionDistance;
            if (NavMesh.SamplePosition(biasedTarget, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
                agent.SetDestination(hit.position);
        }
        // Immediately go back to wander
        EnterState(AIState.Wander);
    }

    void ReactToEasterEggUpdate()
    {
        GameObject egg = FindClosestWithTag("EasterEgg");
        if (egg)
            transform.LookAt(new Vector3(egg.transform.position.x, transform.position.y, egg.transform.position.z));
    }

    void FaceClosestNPC()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject && hit.GetComponent<NavMeshWander>())
            {
                Vector3 dir = hit.transform.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);
                break;
            }
        }
    }

    void EnterState(AIState newState, float duration = 0f)
    {
        // Stop movement on non-wander
        if (newState != AIState.Wander)
            agent.ResetPath();

        currentState = newState;
        CancelInvoke();
        if (duration > 0f)
            Invoke(nameof(ExitState), duration);
    }

    void ExitState()
    {
        // Reset animations
        anim.SetBool("isTalking", false);
        anim.SetBool("isClapping", false);
        currentState = AIState.Wander;
        timer = waitTime;
        GoToDestination(currentWanderTarget);
    }

    void PickNewWanderTarget()
    {
        float biasStrength = 2f; // semakin besar, semakin dekat ke tengah
        float biasedRadius = wanderRadius * Mathf.Pow(Random.value, biasStrength);
        Vector3 randDir = Random.insideUnitSphere * biasedRadius + wanderCenter.position;

        if (NavMesh.SamplePosition(randDir, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            currentWanderTarget = hit.position;
        }

        GoToDestination(currentWanderTarget);
    }

    void GoToDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
    }

    GameObject FindClosestWithTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        foreach (var o in objs)
        {
            float d = Vector3.Distance(transform.position, o.transform.position);
            if (d < minDist)
            {
                closest = o;
                minDist = d;
            }
        }
        return closest;
    }

    void Animate()
    {
        bool walking = agent.velocity.magnitude > 0.1f && agent.hasPath;
        SetAnimationFlags(walking && currentState == AIState.Wander,
                          currentState == AIState.TalkNPC,
                          currentState == AIState.ReactToEasterEgg);
    }

    void SetAnimationFlags(bool walk, bool talk, bool clap)
    {
        anim.SetBool("isWalking", walk);
        anim.SetBool("isTalking", talk);
        anim.SetBool("isClapping", clap);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidPlayerDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectEasterEggRadius);
        if (wanderCenter != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(wanderCenter.position, wanderRadius);
        }
    }
}