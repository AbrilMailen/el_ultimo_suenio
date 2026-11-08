using UnityEngine;

// Datos de un tipo de enemigo. Cambiando el asset cambia el enemigo, sin tocar código.
public class EnemyData : ScriptableObject
{
    [SerializeField] private float _maxHealth = 30f;
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackCooldown = 1.2f;
    [SerializeField] private float _damage = 5f; // segundos de sueño que resta

    public float MaxHealth => _maxHealth;
    public float MoveSpeed => _moveSpeed;
    public float DetectionRange => _detectionRange;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public float Damage => _damage;
}
