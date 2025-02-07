using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Al generarse cada bala se incrementa la cuenta y se actualiza el texto
    void Start() {
        GameObject.Find("GameManager").GetComponent<GameManager>().disparos++;
        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateDisparos();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            GameObject.Find("GameManager").GetComponent<GameManager>().death++;
            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateDeath();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
