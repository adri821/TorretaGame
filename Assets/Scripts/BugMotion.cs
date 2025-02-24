using UnityEngine;
using System.Collections;

public class BugMotion : MonoBehaviour
{
    [SerializeField] public ParticleSystem deathParticles;

    [SerializeField] float circleAttackRadius = 0.1f; // Radio del collider de ataque
    [SerializeField] GameObject hitPoint; // Referencia al punto de ataque
    [SerializeField] int damageValue = 1; // Cantidad de daño que hace el enemigo

    [SerializeField] public float bugSpeed = 5; // Variables privadas accesibles desde el inspector
    [SerializeField] GameObject target; // Referencias privadas accesibles desde el inspector

    // Variable para controlar la longitud de la línea
    [SerializeField] float detectionDistance = 0.5f;

    // Variable para controlar el offset de comienzo de la línea y que no se solape con el collider del bicho
    [SerializeField] float offsetDistance = 0.2f;

    // Cantidad de tiempo que se pausa la corrutina antes de empezar a restar vida
    [SerializeField] float attackDelay = 0.68f;

    // Variable privada para garantizar que una misma instancia del prefab
    // sólo sea capaz de lanzar una única corrutina WaitAndAttack() al mismo tiempo.
    bool executingCoroutine = false;

    // Método que devuelve True si el bicho detecta al jugador
    bool IsDetectingPlayer() {
        // Calculamos el vector dirección normalizado entre la posición actual y la posición del objetivo
        Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

        // Calculamos el punto final de la línea utilizando el vector dirección y la longitud deseada
        Vector2 endPoint = (Vector2)transform.position + direction * detectionDistance;

        // Dibujamos la línea RayCastHit2D teniendo en cuenta que no se solape el comienzo del collider propio
        Debug.DrawLine((Vector2)transform.position + direction * offsetDistance, endPoint, Color.black);

        // Generamos el RayCastHit2D coincidente con el DrawLine (los parámetros cambian un poco).
        // El primer parámetro es el origen + el offset, el segundo parámetro es la dirección
        // (en DrawLine era el punto de finalización), el tercer parámetro es la longitud del
        // rayo con el offset restado
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2)transform.position + direction *
                                            offsetDistance, direction, detectionDistance - offsetDistance);

        // Preguntamos si el RayCastHit2D está colisionando
        if (raycastHit2D.collider != null) {
            if (raycastHit2D.collider.gameObject.CompareTag("Player")) {
                return true; // Como ha detectado al jugador, devuelve true
            }
        }
        // Devolvemos el valor por defecto si no detecta al jugador
        return false;
    }

    // Método Start() del script BugMotion
    private void Start() {
        // Cargamos la referencia al objeto que tenga la etiqueta Player
        // (previamente deberíamos haber asociado dicha etiqueta a la torreta)
        deathParticles = GameObject.Find("DeathParticles").GetComponent<ParticleSystem>();
        target = GameObject.FindGameObjectWithTag("Player");
        bugSpeed += bugSpeed + (Time.time * 0.1f);
    }

    private void Update() {
        // Restamos las posiciones del enemigo y del objetivo
        // para calcular la dirección hacia la que debe mirar (normalizada = magnitud 1)
        Vector2 direction = (target.transform.position - transform.position).normalized;
        transform.up = direction; // Hacemos que el enemigo mire al objetivo en todo momento

        // Llamamos al método que dibuja la línea del RayCastHit2D y devuelve true si ha llegado al
        // destino, false en caso contrario. Por lo tanto, se moverá mientras no detecte al jugador
        if (!IsDetectingPlayer()) GetComponent<Rigidbody2D>().linearVelocity = direction * bugSpeed;
        else {
            SetAnimation("attacking");
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            // ---------------------------------------------------------------------------
            // Si ha detectado al jugador se lanza la corrutina de Espera y Ataque siempre
            // que dicha corrutina no esté ya en ejecución para la instancia del prefab
            if (!executingCoroutine) StartCoroutine(WaitAndAttack());
        }
    }

    // -----------------------------------------------------------------------------
    // Método que desactiva todos los parámetros del Animator y activa uno concreto.
    // Hay que tener en cuenta que el componente Animator está en el objeto hijo
    // y por eso se utiliza GetComponentInChildren<> en vez de GetComponent<>
    // -----------------------------------------------------------------------------
    public void SetAnimation(string name) {
        // Obtenemos todos los parámetros del Animator que están en el objeto hijo
        AnimatorControllerParameter[] parametros = this.GetComponentInChildren<Animator>().parameters;

        // Recorremos todos los parámetros y los ponemos a false
        foreach (var item in parametros) GetComponentInChildren<Animator>().SetBool(item.name, false);

        // Activamos el pasado por parámetro
        GetComponentInChildren<Animator>().SetBool(name, true);
    }

    // Método para generar el collider del golpe y detectar la colisión
    public void Hit() {
        Collider2D collider = Physics2D.OverlapCircle(hitPoint.transform.position, circleAttackRadius);
        if (collider != null) {
            // Si el collider está posicionado sobre el Player
            if (collider.CompareTag("Player")) {
                // Restamos vida al Player llamando al método TakeDamage con el valor
                GameObject.Find("GameManager").GetComponent<GameManager>().TakeDamage(damageValue);
            }
        }
    }

    // Método que dibuja la forma del collider para que sea visible
    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(hitPoint.transform.position, circleAttackRadius);
    }

    // Corrutina para atacar con delay
    public IEnumerator WaitAndAttack() {
        // Indica que la corrutina está en ejecución para bloquear
        // la posibilidad de que la misma instancia del prefab duplique
        // la corrutina
        executingCoroutine = true;

        // Pausamos el tiempo especificado en la variable attackDelay
        yield return new WaitForSeconds(attackDelay);

        // Restamos la vida
        Hit();

        // Restablece la variable de control para posibilitar
        // el lanzamiento de la corrutina otra vez desde el mismo prefab
        executingCoroutine = false;
    }

    // -----------------------------------------------------------------
    // Método que detecta las colisiones de los enemigos con las trampas
    // -----------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision) {
        // Si existe la colisión
        if (collision != null) {
            // Si la colisión es con una trampa
            if (collision.collider.CompareTag("Trap")) {
                deathParticles.transform.position = transform.position;
                // Reproducir el sistema de partículas
                deathParticles.Play();
                // Se incrementa el número de muertes
                GameObject.Find("GameManager").GetComponent<GameManager>().death++;
                // Se actualiza el texto
                GameObject.Find("GameManager").GetComponent<GameManager>().UpdateDeath();
                // NO se llama al generador de objetos aleatorios porque si no es muy fácil...
                // Se destruye el bicho
                Destroy(gameObject);
            }
        }
    }
}
