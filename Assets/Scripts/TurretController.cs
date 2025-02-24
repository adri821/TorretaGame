using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    // Referencias privadas accesibles desde el Inspector
    [SerializeField] GameObject bulletPrefab; // Prefab con la bala
    [SerializeField] GameObject spawnPoint; // Posici�n desde la que instanciar balas
    [SerializeField] float bulletSpeed = 10;

    // Variables privadas
    Vector2 mousePosition; // Para almacenar las coordenadas en p�xeles del mouse
    Vector2 worldMousePosition; // Para almacenar las coordenadas equivalentes del mundo
    Vector2 pointDirection; // Para almacenar el vector de direcci�n que apunta al mouse

    private bool turretCanShoot; // Para almacenar si se puede disparar o no

    private void Update() {
        //GameObject.Find("GameManager").GetComponent<GameManager>().EnableFire();
        mousePosition = Input.mousePosition; // Coordenadas de pantalla

        // Transformamos las coordenadas de pantalla en coordenadas del mundo (dentro del Update)
        worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Hacemos que el GameObject apunte a la posici�n del mouse
        // calculando el vector que contiene la direcci�n hacia el mouse (dentro del Update)
        pointDirection = worldMousePosition - (Vector2)transform.position;

        // Ajustamos la rotaci�n del objeto para que apunte hacia la direcci�n del rat�n
        // Recordatorio: un vector normalizado es un vector que tiene magnitud 1 (dentro del Update)
        transform.up = pointDirection.normalized;

        if (Input.GetMouseButtonDown(0)) {
            StartCoroutine(checkToShoot());
        }
    }

    IEnumerator checkToShoot() {
        yield return new WaitForEndOfFrame();
        // Obtenemos el valor del GameManager que controla si se puede disparar o no
        turretCanShoot = GameObject.Find("GameManager").GetComponent<GameManager>().GetShootingStatus();
        // Detectamos si se ha hecho click con el bot�n primario del rat�n
        // En ese caso instanciamos la bala en la posici�n de spawn
        if (turretCanShoot) {
            // Instanciamos la bala en la posici�n de spawn
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.transform);
            // Eliminamos la dependencia del objeto padre spawnPoint
            bullet.transform.SetParent(null);
            // Configuramos la velocidad de movimiento
            bullet.GetComponent<Rigidbody2D>().linearVelocity = pointDirection.normalized * bulletSpeed;
        }
    }

    // Cuando las balas salen de la escena se autodestruyen
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
