using System;
using System.Collections;
using Controller.Components.VitalitySystem;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : HealthComponent, IMovable
{
    public GameObject wp;
    
    [SerializeField] private float despawnTimer;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    private NavMeshAgent agent;
    public bool reachedWaypoint = false;

    //Temporary standin for player
    public Vector3 playerPos;
    public bool alive = true;
    
    public bool isDead;

    private bool _isMoving;

    [SerializeField] private AudioClip zombieDeathAudio;
    [SerializeField] private AudioClip zombieHeadShotAudio;
    [SerializeField] private AudioClip zombieStunnedAudio;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        isDead = false;
        ResetHealth();
        Move();
        
    }
    // Update is called once per frame
    void Update()
    {

        transform.LookAt(new Vector3(12, transform.position.y, transform.position.z));

        if (!_isMoving)
            return;
        
        if (agent != null && !isDead && wp != null)
        {
            if (!reachedWaypoint)
            {
                //When unit reaches initial waypoint, turn and move in a stright line towards the end of the corridor
                if (Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z),new Vector3(wp.transform.position.x,0,wp.transform.position.z)) < 0.01)
                {
                    reachedWaypoint = true;
                    agent.SetDestination(new Vector3(12, transform.position.y, transform.position.z));
                }
            }
            else
            {
                //If unit is at the end of the corridor/in player attack zone, move to player
                if (transform.position.x >= 10)
                {
                    agent.SetDestination(playerPos);
                    //transform.LookAt(playerPos, Vector3.up);
                }

                else
                {
                    agent.SetDestination(new Vector3(12, transform.position.y, transform.position.z));
                    //transform.LookAt(new Vector3(12, transform.position.y, transform.position.z), Vector3.up);
                }
            }
        }
        if(alive == false)
        {
            StartCoroutine(ZombieDie(0));
            alive = true;
        }
    }

    //Set initial waypoint
    public void SetWaypoint(GameObject waypoint)
    {
        agent.SetDestination(waypoint.transform.position);
        wp = waypoint;
    }

    public IEnumerator ZombieDie(int DeathType)
    {
        isDead = true;
        _isMoving = false;
        agent.isStopped = true;
        foreach (var component in this.GetComponents<Collider>())
        {
            component.isTrigger = true;
        }
        if (DeathType == 0)
        {
            if (zombieDeathAudio != null)
            {
                AudioManager.Instance.PlaySound(zombieDeathAudio);
            }
        }
        else
        {
            if (zombieHeadShotAudio != null)
            {
                AudioManager.Instance.PlaySound(zombieHeadShotAudio);
            }
        }

        yield return new WaitForSeconds(despawnTimer);
        Destroy(this.gameObject);
    }

    public override void DoDamage(DamageData contact, int damage)
    {
        if(isDead)
            return;
        
        if (contact.Impulse.magnitude > toppleForce)
        {
           base.DoDamage(contact, damage);
           base.DoKick(new DamageData()
           {
               Type = ComboType.None,
               Impulse = -0.01f * contact.Impulse
           }, null);
        }
    }

    protected override void OnDeath()
    {
        StartCoroutine(ZombieDie(0));
        _animator.SetTrigger("Death");
    }

    protected override void OnHeadshot()
    {
        StartCoroutine(ZombieDie(1));
        _animator.SetTrigger("Headshot");
    }

    protected override void onPunch()
    {
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (isDead)
        {
            Zombie hitZombie = collision.gameObject.GetComponent<Zombie>();
            if (hitZombie != null)
            {
                if (collision.impulse.magnitude > hitZombie.toppleForce/2)
                    if (!hitZombie.isDead)
                    Debug.Log("Collision Enter");
                    StartCoroutine(hitZombie.ZombieDie());
            }
        }
    }*/
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.GetComponent<Controller.Components.BallComponent>() == null)
    //         return;
    //     Debug.Log($" Enter {other.gameObject.name}" + $" From {gameObject.name}");
    //     HealthComponent health = gameObject.GetComponent<HealthComponent>();
    //     float velocity = other.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
    //     if (health != null && velocity > 0.1)
    //     {
    //         health.DoDamage(velocity * 5, 1);
    //     }
    // }

    protected override void OnDamage()
    {
    }

    protected override void OnKick(Vector3 force, Action callback)
    {
        // rb.useGravity = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(force, ForceMode.Impulse);
        _animator.SetTrigger("Knock");
        StartCoroutine(Delay(0.5f, callback));
    }

    private IEnumerator Delay(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        // rb.useGravity = false;
        _rigidbody.isKinematic = true;
        callback?.Invoke();
    }

    public void Move()
    {
        if(isDead)
            return;
        
        _isMoving = true;
        _animator.SetTrigger("Move");
        _animator.speed = 1;
        if(agent != null)
            agent.isStopped = false;
    }

    public void StopMove()
    {
        _isMoving = false;
        _animator.speed = 0.5f;
        if(agent!=null)
            agent.isStopped = true;
    }
}

public interface IMovable
{
    void Move();
    void StopMove();
}
