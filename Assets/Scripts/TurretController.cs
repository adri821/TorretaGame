using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    // Referencias privadas accesibles desde el Inspector
    [SerializeField] GameObject bulletPrefab; // Prefab con la bala
    [SerializeField] GameObject spawnPoint; // Posición desde la que instanciar balas
    [SerializeField] float bulletSpeed = 10;

    // Variables privadas
    Vector2 mousePosition; // Para almacenar las coordenadas en píxeles del mouse
    Vector2 worldMousePosition; // Para almacenar las coordenadas equivalentes del mundo
    Vector2 pointDirection; // Para almacenar el vector de dirección que apunta al mouse

    private bool turretCanShoot; // Para almacenar si se puede disparar o no

    private void Update() {
        //GameObject.Find("GameManager").GetComponent<GameManager>().EnableFire();
        mousePosition = Input.mousePosition; // Coordenadas de pantalla

        // Transformamos las coordenadas de pantalla en coordenadas del mundo (dentro del Update)
        worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Hacemos que el GameObject apunte a la posición del mouse
        // calculando el vector que contiene la dirección hacia el mouse (dentro del Update)
        pointDirection = worldMousePosition - (Vector2)transform.position;

        // Ajustamos la rotación del objeto para que apunte hacia la dirección del ratón
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
        // Detectamos si se ha hecho click con el botón primario del ratón
        // En ese caso instanciamos la bala en la posición de spawn
        if (turretCanShoot) {
            // Instanciamos la bala en la posición de spawn
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
