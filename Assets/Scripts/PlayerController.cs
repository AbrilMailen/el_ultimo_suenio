using UnityEngine;

// Mueve a la niña con WASD y la gira hacia el mouse (cámara cenital fija)
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Camera _camera;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;
    private Vector3 _aimDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _aimDirection = transform.forward;

        if (_camera == null)
            _camera = Camera.main;
    }

    private void Update()
    {
        ReadMovementInput();
        ReadAimInput();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(Quaternion.LookRotation(_aimDirection));
    }

    private void ReadMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal"); // A / D
        float z = Input.GetAxisRaw("Vertical");   // W / S

        // normalized para que la diagonal no sea más rápida
        _moveDirection = new Vector3(x, 0f, z).normalized;
    }

    private void ReadAimInput()
    {
        // Rayo desde la cámara hasta un plano a la altura de la jugadora
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 direction = ray.GetPoint(distance) - transform.position;
            direction.y = 0f;

            // evita girar de golpe si el mouse está justo encima de ella
            if (direction.sqrMagnitude > 0.01f)
                _aimDirection = direction.normalized;
        }
    }
}
