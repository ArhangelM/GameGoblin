using Assets.Scripts.Common.SignalBus;
using Assets.Scripts.Common.SignalBus.AllSignals;
using Assets.Scripts.Interfaces;
using UnityEngine;

public class Player : MonoBehaviour, IStats
{
    [SerializeField]
    private float _hp;
    [SerializeField]
    private float _attackSpeed = 2;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private Animator _animatorController;

    private Weapon _weapon; 
    private Vector3 _moveVector = Vector3.zero;
    private float _lastAttackTime = 0;
    private bool _activeUlta = true;
    private float _speed = 5;
    private float _distanceForUlta = 5;
    private bool isDead = false;


    public float Hp { get => _hp; set => _hp = value; }
    public float AtackSpeed { get => _attackSpeed; set => _attackSpeed = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }

    private void Start()
    {
        _weapon = GetComponentInChildren<Weapon>();
    }
    private void Update()
    {
        if (isDead)
            return;

        if (Hp <= 0)
        {
            Die();
            return;
        }

        Move();
        UIInvoke();
    }
    private void UIInvoke()
    {
        if (_activeUlta)
        {
            SignalBus.Instance.Invoke(new ChangedTimeSignal { Time = 0 });
            var enemies = SceneManager.Instance.Enemies;

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemie = enemies[i];
                if (enemie == null)
                {
                    continue;
                }

                var distance = Vector3.Distance(transform.position, enemie.transform.position);

                if (distance < _distanceForUlta)
                {
                    SignalBus.Instance.Invoke(new ActiveUltaSignal { IsActive = true });
                    break;
                }
                else
                    SignalBus.Instance.Invoke(new ActiveUltaSignal { IsActive = false });
            }
        }
        else
        {
            SignalBus.Instance.Invoke(new ActiveUltaSignal { IsActive = false });
            var time = _attackSpeed - (Time.time - _lastAttackTime);

            if (time <= AtackSpeed && time >= 0)
                SignalBus.Instance.Invoke(new ChangedTimeSignal { Time = time });
            else
                _activeUlta = true;
        }
    }

    private void Die()
    {
        isDead = true;
        _animatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }

    private void Move()
    {
        _moveVector.x = Input.GetAxis("Horizontal");
        _moveVector.z = Input.GetAxis("Vertical");
        if (_moveVector.x != 0 || _moveVector.z != 0)
        {
            var direction = Vector3.RotateTowards(transform.position, _moveVector, _speed, 0);
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (_moveVector.x != 0 || _moveVector.z != 0) 
            _animatorController.SetBool("Move", true);
        else
            _animatorController.SetBool("Move", false);

        transform.position += new Vector3(_moveVector.x * _speed * Time.deltaTime, 0, _moveVector.z * _speed * Time.deltaTime);
    }

    public void Attack()
    {
        _weapon.Damage = 10;
        _animatorController.SetTrigger("Attack");
    }

    public void Ulta()
    {
        if (Time.time - _lastAttackTime > AtackSpeed)
        {
            _activeUlta = false;
            _lastAttackTime = Time.time;

            _weapon.Damage = 15;
            _animatorController.SetTrigger("Ulta");
        }
    }
}
