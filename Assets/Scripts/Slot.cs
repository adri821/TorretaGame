using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory inventario; // Para guardar la referencia al script del inventario
    [SerializeField] int slotIndex; // Para indicar el slot con el que se vincula

    private void Start() {
        // Obtenemos la referencia al script del Inventario que hay asociado al GameManager
        inventario = GameObject.Find("GameManager").GetComponent<Inventory>();
    }

    void Update() {
        // Comprobamos si el slot ya no tiene objetos hijos y en caso de que no los tenga
        // marcamos la posici�n del vector isFull como FALSE (esto requiere configurar
        // cada Slot con el �ndice correspondiente de isFull).
        if (transform.childCount <= 0) inventario.isFull[slotIndex] = false;
    }

    // M�todo p�blico para desechar objetos del inventario (es p�blico porque lo
    // llamaremos desde un bot�n que no tiene dicho script como componente)
    public void DropItem() {
        // Recorre todos los hijos vinculados al objeto padre
        // que contiene este script y los destruye
        foreach (Transform child in transform) Destroy(child.gameObject);
    }

}
