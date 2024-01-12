using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FreddieMovementNavMesh : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] Transform spawnPosition;
    GameObject player;
    NavMeshAgent freddieAgent;
    int waypointIndex;
    float timer;
    Vector3 target;
    bool hasPotion;



    private void Start()
    {
        freddieAgent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawnPosition.position;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Vector3.Distance(transform.position, target) < 1)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }

        if(Vector3.Distance(transform.position, player.transform.position) < 17)
        {
            freddieAgent.SetDestination(player.transform.position);
        }

        else if(Vector3.Distance(transform.position, player.transform.position) > 17)
        {
            UpdateDestination();
        }

        if(timer > 60)
        {
            SceneManager.LoadScene(1);
        }
    }

    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        freddieAgent.SetDestination(target);
    }

    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            freddieAgent.isStopped = true;
            freddieAgent.velocity = Vector3.zero;
            transform.LookAt(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            freddieAgent.isStopped = false;
        }
    }
}
