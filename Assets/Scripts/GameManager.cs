using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Referencias a objetos privados visibles desde el Inspector
    [SerializeField] GameObject HPBar; // Para referenciar el relleno de la barra de vida

    // Variable p�blica para modificar desde cualquier script
    public float life = 100; // Vida que tiene el jugador en un momento determinado
    public float maxLife = 100; // Vida m�xima del jugador

    [SerializeField] Texture2D cursorTarget;

    // Referencias a objetos privados visibles desde el Inspector
    [SerializeField] TextMeshProUGUI textoDisparos;
    [SerializeField] TextMeshProUGUI textDeathBugs;
    [SerializeField] TextMeshProUGUI textTiempo;

    private float tiempoInicio;

    // Referencias a objetos privados visibles desde el Inspector
    [SerializeField] GameObject dialoguesObject;

    private Inventory inventario; // Para guardar la referencia al script del inventario

    // Referencias a objetos privados visibles desde el Inspector
    [SerializeField] GameObject itemButton_1;
    [SerializeField] GameObject itemButton_2;

    // Variable p�blica para modificar desde cualquier script
    public int disparos = 0;
    public int death = 0;

    // Variables privadas
    private bool canShoot = true;
    private bool firtsItem1 = true;
    private bool firtsItem2 = true;



    private void Start() {
        // Configuramos el cursor con el sprite de la mirilla
        Vector2 hotspot = new Vector2(cursorTarget.width / 2, cursorTarget.height / 2);
        Cursor.SetCursor(cursorTarget, hotspot, CursorMode.Auto);
        tiempoInicio = Time.time;
        // Actualizamos la informaci�n
        UpdateDisparos();

        // Obtenemos la referencia al script del Inventario que hay asociado al GameManager
        inventario = GetComponent<Inventory>();
    }

    // ---------------------------------------------
    // DETECCI�N DE CLICS DEL MOUSE SOBRE LOS ITEMS
    // ---------------------------------------------
    void Update() {
        // Actualizaci�n de la barra de vida
        HPBar.GetComponent<Image>().fillAmount = life / maxLife;

        float tiempoTranscurrido = Time.time - tiempoInicio;

        int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60);
        int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);

        textTiempo.text = string.Format("Tiempo: {0:00}:{1:00}", minutos, segundos);

        // Verificar si se ha hecho clic con el bot�n izquierdo del rat�n
        if (Input.GetMouseButtonDown(0)) {
            // Obtener la posici�n del clic en la pantalla
            Vector3 clicPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Crear un RaycastHit2D para almacenar la informaci�n de colisi�n 2D
            // Desde la posici�n en la que se hizo clic en la direcci�n 0, 0, 0
            // por lo tanto el Raycast ser� un diminuto punto que detecta la colisi�n
            RaycastHit2D hit = Physics2D.Raycast(clicPosition, Vector2.zero);

            // Verificar si ha golpeado un objeto
            if (hit.collider != null) {
                // Filtrar los objetos que interesan seg�n su etiqueta
                if (hit.collider.CompareTag("spikedball_item") && !dialoguesObject.activeSelf) {
                    Debug.Log("��BOLA CON PINCHOS!!");
                    // Verificamos si existen huecos libres en el inventario
                    for (int i = 0; i < inventario.slots.Length; i++) {
                        if (!inventario.isFull[i]) { // Se pueden a�adir items
                            inventario.isFull[i] = true; // Ocupamos la posici�n
                                                         // Instanciamos un bot�n en la posici�n del slot
                            Instantiate(itemButton_1, inventario.slots[i].transform, false);
                            Destroy(hit.collider.gameObject); // Destruimos el objeto firtsItem1
                            DisableFire();
                            if (firtsItem1) {
                                dialoguesObject.SetActive(true); // Activamos los di�logos
                                GameObject.Find("Panel").GetComponent<DialogueController>().StartDialogue("spikedball_item");
                                firtsItem1 = false;
                            }                            
                            break; // Salimos del bucle
                        }
                    }
                }
                if (hit.collider.CompareTag("sawblade_item") && !dialoguesObject.activeSelf) {
                    Debug.Log("��DISCO DE SIERRA!!");
                    for (int i = 0; i < inventario.slots.Length; i++) {
                        if (!inventario.isFull[i]) { // Se pueden a�adir items
                            inventario.isFull[i] = true; // Ocupamos la posici�n
                                                         // Instanciamos un bot�n en la posici�n del slot
                            Instantiate(itemButton_2, inventario.slots[i].transform, false);
                            Destroy(hit.collider.gameObject); // Destruimos el objeto
                            DisableFire();                            
                            if (firtsItem2) {
                                dialoguesObject.SetActive(true); // Activamos los di�logos
                                GameObject.Find("Panel").GetComponent<DialogueController>().StartDialogue("sawblade_item");
                                firtsItem2 = false;
                            }
                            break; // Salimos del bucle
                        }
                    }
                }
                if (hit.collider.CompareTag("Enemy")) {
                    EnableFire();
                }
            } else {
                EnableFire();
            }
        }
        Derrota();
    }

    // M�todo p�blico que actualiza el texto de los disparos
    public void UpdateDisparos() {
        // Actualizamos el texto de las balas disparadas
        textoDisparos.text = "Disparos: " + disparos;
    }

    public void UpdateDeath() {
        textDeathBugs.text = "Muertes: " + death;
    }

    // ---------------------------------------
    // M�todo para desactivar los disparos
    // ---------------------------------------
    public void DisableFire() {
        canShoot = false;
    }

    // ---------------------------------------
    // M�todo para activar los disparos
    // ---------------------------------------
    public void EnableFire() {
        canShoot = true;
    }

    // -------------------------------------------------
    // M�todo para devolver si se puede disparar o no
    // -------------------------------------------------
    public bool GetShootingStatus() {
        return canShoot;
    }

    // M�todo para restar vida al Player
    public void TakeDamage(int damage) {
        life = Mathf.Clamp(life - damage, 0, maxLife);
    }

    // M�todo para a�adir vida al Player
    public void Heal(int lifeRecovered) {
        life = life = Mathf.Clamp(life + lifeRecovered, 0, maxLife); ;
    }

    public void Derrota() {
        if (life == 0) {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            TimePass.instance.tiempoFinal = textTiempo.text;
            SCManager.instance.LoadScene("Derrota");
        }
    }
}
