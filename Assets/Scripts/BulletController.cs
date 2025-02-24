using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Al generarse cada bala se incrementa la cuenta y se actualiza el texto
    void Start() {
        GameObject.Find("GameManager").GetComponent<GameManager>().disparos++;
        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateDisparos();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Si existe la colisión
        if (collision != null) {
            if (collision.gameObject.CompareTag("Enemy")) {
                GameObject.Find(collision.gameObject.name).GetComponent<BugMotion>().deathParticles.transform.position = transform.position;
                GameObject.Find(collision.gameObject.name).GetComponent<BugMotion>().deathParticles.Play();
                GameObject.Find("GameManager").GetComponent<GameManager>().death++;
                GameObject.Find("GameManager").GetComponent<GameManager>().UpdateDeath();
                // Se llama al generador de objetos aleatorios
                GameObject.Find("Instanciador").GetComponent<ItemCreator>().ItemGenerator(collision.gameObject.transform);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
