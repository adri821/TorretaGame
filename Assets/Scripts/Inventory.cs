using UnityEngine;

public class Inventory : MonoBehaviour
{
    // ------------------------------------------------
    // SCRIPT INVENTORY ASOCIADO AL OBJETO GAMEMANAGER
    // ------------------------------------------------
    public bool[] isFull; // Para saber si cada slot est� ocupado
    public GameObject[] slots; // Para referenciar el objeto del slot
}
