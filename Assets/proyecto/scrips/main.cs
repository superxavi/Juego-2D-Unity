using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject menu;
    public GameObject FIN;

    public bool iniciar= false;
    public bool finalizar= false;


    // Fondo y desplazamiento
    public Renderer fondo;
    public float desplazamientoFondo = 0.03f;

    public GameObject dino1Prefab;
    public GameObject dino2Prefab;

    // Configuración para los dinosaurios
    public float intervaloDinosaurios = 2f;
    private float tiempoUltimoDino = 0f;
    public float posicionYMin = -2f;
    public float posicionYMax = 2f;

    // Configuración para las nubes
    public GameObject nubePrefab;
    public int cantidadNubes = 10;
    public float inicioX = 10f;
    public float finX = -10f;
    public float posicionY = -3f;
    public float velocidad = 10f;

    // Lista para manejar las nubes
    private List<GameObject> nubes = new List<GameObject>();
    private List<GameObject> dinosaurios = new List<GameObject>();

    // Sistema de puntuación
    public TextMeshProUGUI scoreText;
    private int score = 0;

    // Prefab de las monedas
    public GameObject monedaPrefab;
    private List<GameObject> monedas = new List<GameObject>();
    public float intervaloMonedas = 2f;
    private float tiempoUltimaMoneda = 0f;
    public float posicionYMin2 = -2f;
    public float posicionYMax2 = 4f;
    public float inicioX2 = 10f;
    public float finX2 = -10f;
    public float velocidad2 = 5f;

    void Start()
    {
        // Inicializar el puntaje
        score = 0;
        ActualizarTextoPuntaje();

        // Crear las nubes iniciales
        for (int i = 0; i < cantidadNubes; i++)
        {
            CrearNubeInicial(i);
        }
    }

    void Update()
    {
        if( iniciar == false )
        {
            if(Input.GetKeyDown(KeyCode.Space)) {
                iniciar = true;
                menu.SetActive(false);
            }

        }


        if ( iniciar == true && finalizar == false)
        {
           
            DesplazarFondo();
            MoverNubes();
            GenerarDinosaurios();
            MoverDinosaurios();
            GenerarMonedas();
            MoverMonedas();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {

            ReiniciarJuego();
        }


    }

    // Método público para incrementar y actualizar el puntaje
    public void IncrementarPuntaje()
    {
        score += 10;
        ActualizarTextoPuntaje();
    }


    private void ActualizarTextoPuntaje()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        // Verifica si el puntaje es 200
        if (score >= 100 && !finalizar)
        {
            FinalizarJuego();
        }

       
    }

    private void FinalizarJuego()
    {
        finalizar = true; // Cambiar el estado del juego a finalizar
        FIN.SetActive(true); // Mostrar el panel de fin del juego
        Debug.Log("¡Fin del juego!"); // Mensaje en la consola (opcional)
        
        

    }
    private void ReiniciarJuego()
    {
        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }








    void DesplazarFondo()
    {
        Vector2 offset = fondo.material.mainTextureOffset;
        offset.x += desplazamientoFondo * Time.deltaTime;
        fondo.material.mainTextureOffset = offset;
    }

    void MoverNubes()
    {
        foreach (GameObject nube in nubes)
        {
            nube.transform.position += Vector3.left * velocidad * Time.deltaTime;
            if (nube.transform.position.x <= finX)
            {
                nube.transform.position = new Vector3(inicioX, posicionY, nube.transform.position.z);
            }
        }
    }

    void CrearNubeInicial(int indice)
    {
        float separacion = (inicioX - finX) / cantidadNubes;
        float posX = inicioX - indice * separacion;
        GameObject nuevaNube = Instantiate(nubePrefab, new Vector3(posX, posicionY, 0), Quaternion.identity);
        nubes.Add(nuevaNube);
    }

    void GenerarDinosaurios()
    {
        if (Time.time - tiempoUltimoDino >= intervaloDinosaurios)
        {
            GameObject prefabDino = Random.value < 0.5f ? dino1Prefab : dino2Prefab;
            float posY = Random.Range(posicionYMin, posicionYMax);
            GameObject nuevoDino = Instantiate(prefabDino, new Vector2(inicioX, posY), Quaternion.identity);
            dinosaurios.Add(nuevoDino);
            tiempoUltimoDino = Time.time;
        }
    }

    void MoverDinosaurios()
    {
        for (int i = dinosaurios.Count - 1; i >= 0; i--)
        {
            dinosaurios[i].transform.position += Vector3.left * Time.deltaTime * velocidad;
            if (dinosaurios[i].transform.position.x <= finX)
            {
                Destroy(dinosaurios[i]);
                dinosaurios.RemoveAt(i);
            }
        }
    }

    void GenerarMonedas()
    {
        if (Time.time - tiempoUltimaMoneda >= intervaloMonedas)
        {
            float posY = Random.Range(posicionYMin2, posicionYMax2);
            GameObject nuevaMoneda = Instantiate(monedaPrefab, new Vector2(inicioX2, posY), Quaternion.identity);
            monedas.Add(nuevaMoneda);
            tiempoUltimaMoneda = Time.time;
        }
    }

    void MoverMonedas()
    {
        for (int i = monedas.Count - 1; i >= 0; i--)
        {
            if (monedas[i] != null)
            {
                monedas[i].transform.position += Vector3.left * Time.deltaTime * velocidad2;
                if (monedas[i].transform.position.x <= finX2)
                {
                    Destroy(monedas[i]);
                    monedas.RemoveAt(i);
                }
            }
        }
    }

    // Método público para eliminar moneda de la lista
    public void EliminarMoneda(GameObject moneda)
    {
        monedas.Remove(moneda);
        Destroy(moneda);
    }
}