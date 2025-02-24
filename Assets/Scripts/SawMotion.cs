using UnityEngine;

public class SawMotion : MonoBehaviour 
{
    // Variables privadas accesibles desde el inspector
    [SerializeField] float sawMotionSpeed = 2.0f;
    [SerializeField] int offsetScreen = 50;

    void Start()
    {
        // Obtenemos la coordenada X del Player
        float xCoord = GameObject.FindGameObjectWithTag("Player").transform.position.x;

        // Posicionamos la trampa a la misma altura del Player
        transform.position = new Vector3(xCoord, transform.position.y, transform.position.z);
    }

    // M�todo que se ejecuta en cada frame del juego
    private void Update() {
        // Movemos el objeto utiliza el m�todo Translate
        transform.Translate(Vector3.right * sawMotionSpeed * Time.deltaTime);

        // Obtenemos la posici�n horizontal de la trampa en p�xeles
        float xPosInPixels = Camera.main.WorldToScreenPoint(transform.position).x;

        // Calculamos si la sierra ha superado el l�mite derecho de la pantalla sum�ndole
        // un offset para darle un margen y que se destruya cuando est� totalmente fuera
        if (xPosInPixels > Screen.width + offsetScreen) Destroy(gameObject);
    }

}
