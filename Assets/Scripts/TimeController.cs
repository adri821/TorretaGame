using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    private void Start() {
        panel.SetActive(true);
        GameObject.FindGameObjectWithTag("TiempoFinal").GetComponent<TextMeshProUGUI>().text = TimePass.instance.tiempoFinal;
    }
}
