using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Texture2D cursorTarget;

    // Referencias a objetos privados visibles desde el Inspector
    [SerializeField] TextMeshProUGUI textoDisparos;

    // Variable pública para modificar desde cualquier script
    public int disparos = 0;

    private void Start() {
        // Configuramos el cursor con el sprite de la mirilla
        Vector2 hotspot = new Vector2(cursorTarget.width / 2, cursorTarget.height / 2);
        Cursor.SetCursor(cursorTarget, hotspot, CursorMode.Auto);

        // Actualizamos la información
        UpdateDisparos();
    }

    // Método público que actualiza el texto de los disparos
    public void UpdateDisparos() {
        // Actualizamos el texto de las balas disparadas
        textoDisparos.text = "Disparos: " + disparos;
    }
}
