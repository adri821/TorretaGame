using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    // -------------------------------------------------------------
    // ATRIBUTOS NECESARIOS PARA IMPLEMENTAR EL SISTEMA DE DI�LOGO
    // -------------------------------------------------------------
    // Velocidad con la que aparecer� el texto
    [SerializeField] float textSpeed = 0.05f;
    [SerializeField] float extraTimeOnScreen = 2f;

    // Las distintas l�neas de texto
    [SerializeField] string[] textLines;

    // �ndice que indica la l�nea de texto a mostrar
    [SerializeField] int lineIndex = 0;

    // Referencia al objeto del texto
    [SerializeField] TextMeshProUGUI dialogueText;

    // M�todo que lanza el sistema de di�logos con la l�nea
    // que se ha pasado como par�metro
    public void StartDialogue(string item) {

        this.gameObject.SetActive(true); // Activamos el objeto dormido
        dialogueText.text = ""; // Se borra todo el texto escrito en pantalla

        // Seleccionamos la frase que corresponda seg�n la string de entrada
        switch (item) {
            case "sawblade_item": lineIndex = 0; break; // Mostramos la frase de la sierra
            case "spikedball_item": lineIndex = 1; break; // Mostramos la frase de la bola
            default: break;
        }

        StartCoroutine(WriteLine()); // Lanzamos la corrutina
    }

    // M�todo que desactiva el objeto que lleva el sistema de di�logos
    public void EndDialogue() {
        gameObject.SetActive(false); // Desactivamos el objeto
    }

    // -------------------------------------------------
    // CORRUTINA QUE MUESTRA CADA L�NEA LETRA POR LETRA
    // -------------------------------------------------
    IEnumerator WriteLine() {
        // Recorremos cada letra de la l�nea de texto
        foreach (var letra in textLines[lineIndex].ToCharArray()) {
            // Concatenamos las letras en el GameObject una a una
            dialogueText.text += letra;
            // Esperamos hasta la siguiente letra
            yield return new WaitForSeconds(textSpeed);
        }
        // Cuando ya se ha mostrado la frase entera, la dejamos un tiempo en pantalla
        yield return new WaitForSeconds(extraTimeOnScreen);
        EndDialogue(); // Desactivamos el sistema de di�logos
    }

    private void Start() {
        // Desactivamos los di�logos al arrancar el juego
        // por si estaban activados por defecto
        EndDialogue();
    }
}
