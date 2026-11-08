using UnityEngine;

// poner en la Main Camera, target = el player
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 20f, 0f);
    [SerializeField] private float _followSpeed = 8f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 desiredPosition = _target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);
    }
}
