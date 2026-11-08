using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Dead
}

// Base de todos los enemigos: detectar -> perseguir -> atacar -> recibir daño -> morir.
// Los hijos solo definen CÓMO atacan (PerformAttack).
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData _data;
    [SerializeField] private Transform _target; // la jugadora

    protected EnemyData Data => _data;
    protected IDamageable TargetDamageable { get; private set; }

    private NavMeshAgent _agent;
    private EnemyState _state = EnemyState.Idle;
    private float _health;
    private float _nextAttackTime;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _data.MoveSpeed;
        _agent.stoppingDistance = _data.AttackRange * 0.8f;
        _health = _data.MaxHealth;
    }

    private void Start()
    {
        // La jugadora implementa IDamageable a través de su DreamTimer
        if (_target != null)
            TargetDamageable = _target.GetComponent<IDamageable>();
    }

    private void Update()
    {
        if (_target == null || _state == EnemyState.Dead)
            return;

        float distance = Vector3.Distance(transform.position, _target.position);

        switch (_state)
        {
            case EnemyState.Idle:
                if (distance <= _data.DetectionRange)
                    _state = EnemyState.Chase;
                break;

            case EnemyState.Chase:
                UpdateChase(distance);
                break;

            case EnemyState.Attack:
                UpdateAttack(distance);
                break;
        }
    }

    private void UpdateChase(float distance)
    {
        if (distance > _data.DetectionRange)
        {
            _state = EnemyState.Idle;
            StopMoving();
            return;
        }

        if (distance <= _data.AttackRange)
        {
            _state = EnemyState.Attack;
            StopMoving();
            return;
        }

        if (_agent.isOnNavMesh)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
        }
    }

    private void UpdateAttack(float distance)
    {
        // Si la jugadora se alejó, vuelve a perseguirla
        if (distance > _data.AttackRange)
        {
            _state = EnemyState.Chase;
            return;
        }

        // Mirar a la jugadora (solo en el plano horizontal)
        Vector3 look = _target.position - transform.position;
        look.y = 0f;
        if (look.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(look);

        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + _data.AttackCooldown;
            PerformAttack();
        }
    }

    private void StopMoving()
    {
        if (_agent.isOnNavMesh)
            _agent.isStopped = true;
    }

    // Cada tipo de enemigo decide cómo ataca
    protected abstract void PerformAttack();

    public void TakeDamage(float damage)
    {
        if (_state == EnemyState.Dead)
            return;

        _health -= damage;

        if (_health <= 0f)
            Die();
    }

    private void Die()
    {
        _state = EnemyState.Dead;
        StopMoving();

        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        Destroy(gameObject, 0.5f);
    }
}
