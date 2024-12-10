using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    private Animator animator;
    public float velocidadCaminata = 2f;
    public float fuerzaSalto = 5f;
    private Rigidbody2D rb;
    public float velocidadCaidaRapida = 10f;
    private bool estaEnElSuelo = true;
    private Main mainScript;

    // Límites en el eje X
    public float limiteIzquierdo = -10f;
    public float limiteDerecho = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mainScript = FindObjectOfType<Main>();
    }

    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * movimiento * velocidadCaminata * Time.deltaTime);

        // Aplicar límites en el eje X
        float posicionX = Mathf.Clamp(transform.position.x, limiteIzquierdo, limiteDerecho);
        transform.position = new Vector2(posicionX, transform.position.y);

        if (movimiento != 0 && estaEnElSuelo)
        {
            animator.SetBool("caminando", true);
        }
        else if (estaEnElSuelo)
        {
            animator.SetBool("caminando", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && estaEnElSuelo)
        {
            animator.SetBool("caminando", true);
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            estaEnElSuelo = false;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, -velocidadCaidaRapida);
        }

        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow) && !estaEnElSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnElSuelo = true;
            animator.SetBool("caminando", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Moneda"))
        {
            // Incrementar el puntaje
            if (mainScript != null)
            {
                mainScript.IncrementarPuntaje();
                mainScript.EliminarMoneda(other.gameObject);
            }
        }
    }
}
