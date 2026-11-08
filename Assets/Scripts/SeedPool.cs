using UnityEngine;
using UnityEngine.Pool;

// Pool de semillas: reutiliza los mismos objetos en vez de crear y destruir cada disparo
public class SeedPool : MonoBehaviour
{
    [SerializeField] private Seed _seedPrefab;
    [SerializeField] private int _initialSize = 10;
    [SerializeField] private int _maxSize = 20;

    private ObjectPool<Seed> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Seed>(
            createFunc: CreateSeed,
            actionOnGet: seed => seed.gameObject.SetActive(true),
            actionOnRelease: seed => seed.gameObject.SetActive(false),
            actionOnDestroy: seed => Destroy(seed.gameObject),
            collectionCheck: false,
            defaultCapacity: _initialSize,
            maxSize: _maxSize
        );
    }

    public Seed Get(Vector3 position, Quaternion rotation)
    {
        Seed seed = _pool.Get();
        seed.transform.SetPositionAndRotation(position, rotation);
        return seed;
    }

    private Seed CreateSeed()
    {
        Seed seed = Instantiate(_seedPrefab, transform);
        seed.Initialize(_pool);
        return seed;
    }
}
