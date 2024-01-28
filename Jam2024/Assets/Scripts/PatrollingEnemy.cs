using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform Target;
    NavMeshAgent agent;   
    public float Speed;
    public float Stamina;
    public float detectionRadius = 10f;
    public bool EnemyOnArea = false;
    public bool StartPersecution = false;
    public bool FirstMovement = false;
    public float coneAngle = 60f;
    public float coneDistance = 5f;
    public int coneRays = 20;
    public Transform[] PatrolPoints;
    private int currentPatrolPointIndex = 0;
    [SerializeField] private LayerMask _layerMask;
    public virtual void Skill()
    {

    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Persecution()
    {
        agent.speed = Speed;
        agent.SetDestination(Target.position);
    }
    private void Update()
    {
        if (PatrolPoints.Length > 0 && !StartPersecution)
        {
            Patrol();
        }
        else if (StartPersecution)
        {
            Persecution();
        }

    }
    public List<RaycastHit2D> CastCone(Vector2 origin, Vector2 direction, float angle, float distance, int rays)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        float step = angle / (rays - 1);
        for (int i = 0; i < rays; i++)
        {
            float currentAngle = -angle / 2 + step * i;
            Vector2 directionForRay = Quaternion.Euler(0, 0, currentAngle) * direction;
            RaycastHit2D hit = Physics2D.Raycast(origin, directionForRay, distance);
            Debug.DrawLine(origin, origin + directionForRay * distance, Color.yellow, 5f);
            if (hit.collider != null)
            {
                hits.Add(hit);
            }
        }
        return hits;
    }

    private void Patrol()
    {
        if (PatrolPoints.Length == 0)
        {
            return;
        }
        if (agent.remainingDistance <= 0.1f || FirstMovement)
        {
            agent.speed = Speed;
            agent.SetDestination(PatrolPoints[currentPatrolPointIndex].position);
            currentPatrolPointIndex = currentPatrolPointIndex+1 >= PatrolPoints.Length ? 0 :currentPatrolPointIndex + 1;
            FirstMovement = false;
        }
    }

    IEnumerator PerformConeRaycast()
    {
        Vector2 direction = transform.up;
        // Obtener la dirección de movimiento normalizada
        Vector3 agentMovementDirection = agent.velocity.normalized;
        // Determinar la dirección
        if (agentMovementDirection != Vector3.zero)
        {
            if (Mathf.Abs(agentMovementDirection.x) > Mathf.Abs(agentMovementDirection.y))
            {
                if (agentMovementDirection.x > 0)
                {
                    direction = transform.right;
                }
                else
                {
                    direction = transform.right * -1;
                }
            }
            else
            {
                if (agentMovementDirection.y > 0)
                {
                    direction = transform.up;
                }
                else
                {
                    direction = transform.up * -1;
                }
            }
        }
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        List<RaycastHit2D> hits = CastCone(origin, direction, coneAngle, coneDistance, coneRays);
        yield return new WaitForSeconds(0.5f);
        if(hits.Count == 0 && EnemyOnArea)
        {
            StartCoroutine(PerformConeRaycast());
        }
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                StartPersecution = true;
                Target = hit.collider.transform;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(PerformConeRaycast());
            EnemyOnArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(PerformConeRaycast());
            Target = null;
            EnemyOnArea = false;
            StartPersecution = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
