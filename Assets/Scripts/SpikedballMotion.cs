using System;
using UnityEngine;

public class SpikedballMotion : MonoBehaviour
{
    // Referencia privada al objeto ForcePoint
    GameObject forcePointRef;

    // Variable privada accesible desde el inspector para configurar
    // la fuerza con la que se generan los movimientos circulares
    [SerializeField] float rotationForce = 30f;

    // Variable privada accesible desde el inspector para configurar
    // el tiempo de vida de la trampa (en segundos)
    [SerializeField] float lifeSpan = 5f;

    // Variable privada para contabilizar el tiempo total que ha pasado
    // desde que se gener� la instancia de la trampa
    private float time;

    void Start() {
        // Obtenemos la referencia al objeto sobre el
        // que se aplicar� la fuerza
        forcePointRef = GameObject.Find("ForcePoint");
    }

    void Update() {
        // Aplica una fuerza en la direcci�n local que apunta a la derecha (como al
        // cambiar la rotaci�n la direcci�n tambi�n lo hace, funcionar� perfectamente)
        forcePointRef.GetComponent<Rigidbody2D>().linearVelocity = forcePointRef.transform.right * rotationForce;

        // Obtenemos el tiempo que ha pasado desde el anterior
        // frame y lo vamos acumulando en la variable time
        time += Time.deltaTime;

        // Comprobamos si se ha alcanzado el tiempo de vida y si lo hizo se
        // destruir� el gameObject de la trampa
        if (time >= lifeSpan) Destroy(gameObject);
    }
}
