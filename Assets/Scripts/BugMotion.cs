using UnityEngine;

public class BugMotion : MonoBehaviour
{
    [SerializeField] float bugSpeed = 5; // Variables privadas accesibles desde el inspector
    [SerializeField] GameObject target; // Referencias privadas accesibles desde el inspector

    // M�todo Start() del script BugMotion
    private void Start() {
        // Cargamos la referencia al objeto que tenga la etiqueta Player
        // (previamente deber�amos haber asociado dicha etiqueta a la torreta)
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {
        // Restamos las posiciones del enemigo y del objetivo
        // para calcular la direcci�n hacia la que debe mirar (normalizada = magnitud 1)
        Vector2 direction = (target.transform.position - transform.position).normalized;

        transform.up = direction; // Hacemos que el enemigo mire al objetivo en todo momento

        // Ahora que el bicho ya mira todo el tiempo al objetivo
        // hay que iniciar el movimiento del mismo
        GetComponent<Rigidbody2D>().linearVelocity = direction * bugSpeed;
    }
}
