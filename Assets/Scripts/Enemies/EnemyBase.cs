using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour
{
    public int health = 100;
    public int damage = 20;
    public float speed = 3.5f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public Transform target;

    protected NavMeshAgent agent;
    protected float lastAttackTime = 0f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    protected virtual void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            if (Vector3.Distance(transform.position, target.position) <= attackRange && Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    public virtual void TakeDamage(int amount) 
    { 
        health -= amount;
        if (health <= 0)
            Destroy(gameObject);
    }

    protected virtual void Attack() 
    {
        //attack logic here
        Debug.Log($"{gameObject.name} attacks!");
    }
}
