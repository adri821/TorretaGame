using UnityEngine;

public class TimePass : MonoBehaviour {
    public static TimePass instance;

    public string tiempoFinal;
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye entre escenas
        }
        else {
            Destroy(gameObject);
        }
    }
}
