using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    public Transform Player;

    public Transform[] spot;

    public int spotindex = 0;

    private bool foward;


    public enum State { Patrol, Stalk }
    private State state;
    public float Patrolspeed = 10f;
    public float StalkSpeed = 12f;

    //cone de visão
    public float VisionDistance = 45f;
    public float VisionDegrees = 90f;
    public LayerMask layerPlayer;
    public LayerMask layerObstcale;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        state = State.Patrol;
        Patrol();

        navMeshAgent.destination = spot[spotindex].position;
    }

    // Update is called once per frame
    void Update()
    {
        //machine state
        switch (state)
        {
            case State.Patrol:
                navMeshAgent.speed = Patrolspeed;
                Patrol();
                if (SeePlayer())
                {
                    state = State.Stalk;
                    VisionDegrees = 330f;
                }
                break;
            case State.Stalk:
                navMeshAgent.speed = StalkSpeed;
                navMeshAgent.destination = Player.position;
                if (!SeePlayer())
                {
                    state = State.Patrol;
                    VisionDegrees = 90f;
                }
                break;
        }


        //ataque

        if (Vector3.Distance(transform.position, Player.transform.position) < 1.5f)
        {
            navMeshAgent.speed = 0;
            StartCoroutine("ataque");
        }



    }

    //Patrulha
    void Patrol()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {

            if (foward)
            {
                spotindex++;

                if (spotindex >= spot.Length - 1)
                {
                    spotindex = spot.Length - 1;
                    foward = false;
                }
            }
            else
            {
                spotindex--;

                if (spotindex <= 0)
                {
                    spotindex = 0;
                    foward = true;
                }
            }

            navMeshAgent.destination = spot[spotindex].position;
        }
    }

    bool SeePlayer()
    {
        Collider[] alvos = Physics.OverlapSphere(transform.position, VisionDistance, layerPlayer);

        foreach (Collider alvo in alvos)
        {
            Vector3 direction = (alvo.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, direction) < VisionDegrees / 2)
            {
                float distance = Vector3.Distance(transform.position, alvo.transform.position);
                if (!Physics.Raycast(transform.position, direction, distance, layerObstcale))
                {
                    return true;
                }
            }

        }
        return false;
    }

   void OnDrawGizmosSelected()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, VisionDistance);

    Vector3 esquerda = Quaternion.Euler(0, -VisionDegrees / 2, 0) * transform.forward;
    Vector3 direita = Quaternion.Euler(0, VisionDegrees / 2, 0) * transform.forward;

    Gizmos.color = Color.blue;
    Gizmos.DrawRay(transform.position, esquerda * VisionDistance);
    Gizmos.DrawRay(transform.position, direita * VisionDistance);

    
    Collider[] alvos = Physics.OverlapSphere(transform.position, VisionDistance, layerPlayer);

    foreach (Collider alvo in alvos)
    {
        Vector3 origem = transform.position + Vector3.up * 1.5f;
        Vector3 destino = alvo.transform.position + Vector3.up * 1.5f;

        Vector3 direction = (destino - origem).normalized;

        if (Vector3.Angle(transform.forward, direction) < VisionDegrees / 2)
        {
            float distance = Vector3.Distance(origem, destino);

            if (Physics.Raycast(origem, direction, distance, layerObstcale))
            {
                Gizmos.color = Color.red; // bloqueado
            }
            else
            {
                Gizmos.color = Color.green; // vendo o player
            }

            Gizmos.DrawLine(origem, destino);
        }
    }
}

    IEnumerator ataque()
    {
        yield return new WaitForSeconds(2.8f);
        navMeshAgent.speed = StalkSpeed;
    }
}
