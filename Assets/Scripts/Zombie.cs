using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool reachedWaypoint = false;
    //Temporary standin for player
    public Vector3 playerPos;
    public bool alive = true;
    [SerializeField] private float despawnTimer;
    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (agent != null)
        {
            if (!reachedWaypoint)
            {
                //When unit reaches initial waypoint, turn and move in a stright line towards the end of the corridor
                if (agent.remainingDistance <= 0.01)
                {
                    reachedWaypoint = true;
                    agent.SetDestination(new Vector3(12, transform.position.y, transform.position.z));
                }
            }
            else
            {
                //If unit is at the end of the corridor/in player attack zone, move to player
                if (transform.position.x >= 12)
                {
                    agent.SetDestination(playerPos);
                    transform.LookAt(playerPos, Vector3.up);
                }

                else
                {
                    agent.SetDestination(new Vector3(12, transform.position.y, transform.position.z));
                    transform.LookAt(new Vector3(12, transform.position.y, transform.position.z), Vector3.up);
                }
            }
        }
        if(alive == false)
        {
            StartCoroutine(ZombieDie());
            alive = true;
        }
    }

    //Set initial waypoint
    public void SetWaypoint(Vector3 waypoint)
    {
        agent.SetDestination(waypoint);
    }

    public IEnumerator ZombieDie()
    {
        if (agent != null)
            Destroy(agent);   
        //agent.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        //rb.AddForceAtPosition(new Vector3(-300, 0, 0), new Vector3(0,2,0));

        yield return new WaitForSeconds(despawnTimer);

        Destroy(this.gameObject);
    }
}
