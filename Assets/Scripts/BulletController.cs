using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Al generarse cada bala se incrementa la cuenta y se actualiza el texto
    void Start() {
        GameObject.Find("GameManager").GetComponent<GameManager>().disparos++;
        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateDisparos();
    }
}
