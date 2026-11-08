using UnityEngine;
using UnityEngine.Pool;

// Proyectil de semilla. Avanza recto, daña lo que toque y vuelve al pool.
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Seed : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _lifetime = 2f;

    private IObjectPool<Seed> _pool;
    private Transform _owner;
    private float _despawnTime;
    private bool _released;

    private void Awake()
    {
        // Configuración necesaria para que OnTriggerEnter funcione sin física
        Rigidbody body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;
        GetComponent<Collider>().isTrigger = true;
    }

    // Lo llama el SeedPool al crear la semilla
    public void Initialize(IObjectPool<Seed> pool)
    {
        _pool = pool;
    }

    // Lo llama quien dispara. 'owner' es para no golpearse a sí misma
    public void Launch(Transform owner)
    {
        _owner = owner;
        _despawnTime = Time.time + _lifetime;
        _released = false;
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;

        if (Time.time >= _despawnTime)
            Release();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_released || other.isTrigger)
            return;

        if (_owner != null && other.transform.IsChildOf(_owner))
            return;

        other.GetComponentInParent<IDamageable>()?.TakeDamage(_damage);
        Release();
    }

    private void Release()
    {
        if (_released)
            return;

        _released = true;
        _pool.Release(this);
    }
}
