using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform Target;
    NavMeshAgent agent;   
    public float Speed;
    public float Stamina;
    public virtual void Skill()
    {

    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Move()
    {
        agent.speed = Speed;
        agent.SetDestination(Target.position);
    }

    private void Update()
    {
        Move();
    }
}
