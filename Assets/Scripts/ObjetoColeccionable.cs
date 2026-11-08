using UnityEngine;
/*
Detecta cuando Nena se acerca.
Si presiona E, recoge el hongo.
El hongo/cofre desaparece.
*/

public class ObjetoColeccionable : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;

    [Header("Referencias")]
    [SerializeField] private LevelManager _levelManager;

    private bool _jugadorCerca;
    private bool _recogido;

    private void Update()
    {
        if (_jugadorCerca && !_recogido &&
            Input.GetKeyDown(teclaInteraccion))
        {
            RecogerObjeto();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _jugadorCerca = true;
            Debug.Log("Presioná E para recoger el objeto");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _jugadorCerca = false;
        }
    }

    private void RecogerObjeto()
    {
        _recogido = true;

        if (_levelManager != null)
        {
            _levelManager.CollectFlower();
        }
        else
        {
            Debug.LogWarning("Falta asignar LevelManager al objeto coleccionable.");
        }

        Debug.Log("¡Objeto coleccionado!");

        gameObject.SetActive(false);
    }
}