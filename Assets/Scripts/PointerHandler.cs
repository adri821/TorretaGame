using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // Variable privada para controlar si el mouse est� sobre el elemento
    private bool isHover = false;

    // M�todo que detecta cuando se pasa el rat�n por encima del �rea del bot�n
    public void OnPointerEnter(PointerEventData eventData) {
        // El mouse entra en el �rea del bot�n
        isHover = true; // Marcamos la variable para indicar que el mouse se superpone
    }

    // M�todo que detecta cuando sale el bot�n del �rea del rat�n
    public void OnPointerExit(PointerEventData eventData) {
        // El mouse sale del �rea del bot�n
        isHover = false; // Desmarcamos la variable indicando que el mouse ya no se superpone
    }

    // M�todo que se ejecuta una vez en cada frame
    void Update() {
        // Si el mouse est� sobre el elemento se desactivan los disparos hay que mantenerlos
        // desactivados constantemente porque hay otro script que los reactiva
        if (isHover) {
            GameObject.Find("GameManager").GetComponent<GameManager>().DisableFire();
        }
    }
}
