using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour, IStats
{
    [SerializeField]
    private float _hp;
    [SerializeField]
    private float _attackSpeed;
    [SerializeField]
    private float _attackRange = 2;


    public Animator AnimatorController;
    public NavMeshAgent Agent;

    private float lastAttackTime = 0;
    protected bool isDead = false;

    public float Hp { get => _hp; set => _hp = value; }
    public float AtackSpeed { get => _attackSpeed; set => _attackSpeed = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }

    protected void Start()
    {
        SceneManager.Instance.AddEnemie(this);
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);

    }

    protected virtual void Update()
    {
        if(isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            Agent.isStopped = true;
            return;
        }

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
     
        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            Agent.speed = 0;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }

        if (Agent.speed != 0)
            AnimatorController.SetBool("Move", true);
        else
            AnimatorController.SetBool("Move", false);

        Debug.Log(Agent.speed);

    }



    protected virtual void Die()
    {
        SceneManager.Instance.RemoveEnemie(this);
        //¿·Ó SignalBus
        SceneManager.Instance.Player.Hp += 5;
        isDead = true;
        AnimatorController.SetTrigger("Die");
    }

}
