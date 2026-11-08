using UnityEngine;

// Zona de salida del nivel. No decide si se gana: solo avisa al LevelManager
// que la jugadora está en la salida, y él comprueba si tiene la flor.
[RequireComponent(typeof(Collider))]
public class LevelExit : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    // Stay y no Enter: si la jugadora llega sin flor, la recoge y vuelve
    // a la salida sin salir de la zona, igual tiene que poder ganar
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            _levelManager.TryCompleteLevel();
    }
}
