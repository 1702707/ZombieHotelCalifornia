using System.Collections;
using System.Collections.Generic;
using Controller.Components.VitalitySystem;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : HealthComponent
{
    [SerializeField] private float despawnTimer;
    [SerializeField] private Rigidbody _rigidbody;
    
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool reachedWaypoint = false;
    private bool isKicked;
    
    //Temporary standin for player
    public Vector3 playerPos;
    public bool alive = true;

    public float toppleForce;
    public bool isDead;
    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        isDead = false;
        ResetHealth();
    }
    // Update is called once per frame
    void Update()
    {
        if (agent != null && !isDead && !isKicked)
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
        isDead = true;
        if (agent != null)
            Destroy(agent);   
        //agent.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        //rb.AddForceAtPosition(new Vector3(-300, 0, 0), new Vector3(0,2,0));

        yield return new WaitForSeconds(despawnTimer);

        Destroy(this.gameObject);
    }

    public override void DoDamage(float impulse, int damage)
    {
        if(isDead)
            return;
        
        if (impulse > toppleForce)
        {
           base.DoDamage(damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead)
        {
            Zombie hitZombie = collision.gameObject.GetComponent<Zombie>();
            if (hitZombie != null)
            {
                if (collision.impulse.magnitude > hitZombie.toppleForce/2)
                    if (!hitZombie.isDead)
                    StartCoroutine(hitZombie.ZombieDie());
            }
        }
    }

    protected override void OnDamage()
    {
        if (CurrentHP == 0 && !isDead)
        {
            StartCoroutine(ZombieDie());
        }
    }

    protected override void OnKick(Vector3 force)
    {
        isKicked = true;
        // rb.useGravity = true;
        rb.isKinematic = false;
        _rigidbody.AddForce(force, ForceMode.Impulse);
        StartCoroutine(Delay(0.5f));
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isKicked = false;
        // rb.useGravity = false;
        rb.isKinematic = true;
    }
}
