using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FantasyScript : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] GameObject realPoison;
    [SerializeField] GameObject grabbedPoison;
    [SerializeField] GameObject cauldron;
    [SerializeField] GameObject activeWitch;
    [SerializeField] GameObject happyWitch;
    [SerializeField] Transform spawnPosition;
    GameObject player;


    NavMeshAgent witchAgent;
    int waypointIndex;
    float timer;
    Vector3 target;
    Animator witchAnimator;
    bool hasPotion;


    private void Start()
    {
        if (ObjectInteraction.Interaction.fantasySceneFinished)
        {
            activeWitch.SetActive(false);
            happyWitch.SetActive(true);

            cauldron.GetComponent<Collider>().enabled = false;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawnPosition.position;

        witchAgent = GetComponent<NavMeshAgent>();
        witchAnimator = GetComponent<Animator>();
        UpdateDestination();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(Vector3.Distance(transform.position, target) < 1)
        {
            if(waypointIndex == 0)
            {
                witchAnimator.SetTrigger("isLooking");
            }

            if(waypointIndex == 1)
            {
                witchAnimator.SetTrigger("isGrabbing");
                hasPotion = !hasPotion;
                StartCoroutine(DelayThenGrab(1.3f));
                witchAnimator.SetBool("isWalking", true);
            }

            if(waypointIndex == 2)
            {
                witchAnimator.SetTrigger("isCooking");
            }
            IterateWaypointIndex();
            UpdateDestination();
        }

        if(!witchAgent.isStopped)
        {
            witchAnimator.SetBool("isWalking", true);
        }
        else if(witchAgent.isStopped)
        {
            witchAnimator.SetBool("isWalking", false);
        }

        if(timer > 80)
        {
            SceneManager.LoadScene(1);
        }
    }

    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        witchAgent.SetDestination(target);
    }

    void IterateWaypointIndex()
    {
        waypointIndex++;
        if(waypointIndex == waypoints.Length)
        {
            waypointIndex = 1;
        }

        if(waypointIndex == 0)
        {
            StartCoroutine(DelayAndPlayAnimation(5));
        }

        if (waypointIndex == 1)
        {
            StartCoroutine(DelayAndPlayAnimation(8));
        }

        if (waypointIndex == 2)
        {
            StartCoroutine(DelayAndPlayAnimation(5));
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            witchAgent.isStopped = true;
            witchAgent.velocity = Vector3.zero;
            transform.LookAt(collision.transform);
            Debug.Log("Begone pesky fiend!");
            //SceneManager.LoadScene(1);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            witchAgent.isStopped = false;
        }
    }

    IEnumerator DelayAndPlayAnimation(float delay)
    {
        witchAgent.isStopped = true;
        //do animation
        yield return new WaitForSeconds(delay);
        witchAgent.isStopped = false;
    }

    IEnumerator DelayThenGrab(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(hasPotion)
        {
            realPoison.gameObject.SetActive(false);
            grabbedPoison.gameObject.SetActive(true);
        }
        else if(!hasPotion)
        {
            realPoison.gameObject.SetActive(true);
            grabbedPoison.gameObject.SetActive(false);
        }
    }
}
