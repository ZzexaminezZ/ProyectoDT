using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StandEnemy : MonoBehaviour
{
    public Transform Target;
    NavMeshAgent agent;   
    public float Speed;
    public float detectionRadius = 10f;
    public bool StartPersecution = false;
    public bool OnPersecution = false;
    [SerializeField] private LayerMask _layerMask;

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
        if (OnPersecution)
        {
            Persecution();
        }

    }
    IEnumerator CheckPlayer()
    {
        if (StartPersecution && Target != null)
        {
            // Lanzar un raycast desde la posición del enemigo hacia el jugador
            Debug.DrawLine(transform.position, Target.transform.position, Color.yellow, 5f);
            Vector3 directionToPlayer = (Target.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, _layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {   
                    OnPersecution = true;
                }
            }

        }
        yield return new WaitForSeconds(0.1f);
        if(StartPersecution && Target != null && !OnPersecution)
        {
            StartCoroutine(CheckPlayer());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartPersecution = true;
            Target = collision.transform;
            StartCoroutine(CheckPlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartPersecution = false;
            Target = null;
            OnPersecution = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
