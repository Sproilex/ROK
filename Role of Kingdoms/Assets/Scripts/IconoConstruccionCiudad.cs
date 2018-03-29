using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class IconoConstruccionCiudad : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("Informacion de la Construccion")]
    [Tooltip("Nombre del Edificio que se creara")]
    public string NombreEdificio;
    [Tooltip("Prefab de la construccion que sera el que se creara en el mapa")]
    public GameObject PrefabConstruccion;
    [Tooltip("Espacio de anchura (En cuadriculas) que ocupara la construccion")]
    [Range(1, 10)]
    public int AnchuraObjeto;
    [Tooltip("Espacio de altura (En cuadriculas) que ocupara la construccion")]
    [Range(1, 10)]
    public int AlturaObjeto;
    [Tooltip("Icono que aparecera al terminar de construirse el edificio")]
    public Sprite IconoEdificio;
    [Tooltip("Tiempo que tardara la construccion en completarse")]
    [Range(1,1440)]
    public float TiempoConstruccion;
    [Tooltip("Formato en que esta escrito el tiempo anterior")]
    public EFormatoTiempo FormatoTiempoConstruccion;
    [Tooltip("Si esta marcado significa que esta construccion solo se podra construir una unica vez durante todo el juego")]
    public bool EsConstruccionUnica;
    [Tooltip("Numero de veces que se puede construir el edificio")]
    public int CantidadParaConstruir;

    private ManejadorGeneralMundo MGM;

    //Drag and Drop:
    GameObject Construccion;
    RaycastHit2D[] GolpesRays = new RaycastHit2D[10];
    private bool _enCuadricula;
    private List<string> nombresCuadricula;

    //Metodos:

    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        nombresCuadricula = new List<string>();
        if (EsConstruccionUnica)
        {
            CantidadParaConstruir = 1;
        }
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = CantidadParaConstruir.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones").GetComponent<DesplazamientoMapas>().NoMover = true;
        if (CantidadParaConstruir > 0)
        {
            Transform Ciudad = GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones").transform;
            Construccion = Instantiate(PrefabConstruccion, Ciudad.position, new Quaternion(), Ciudad);
            Construccion.name = NombreEdificio + "_" + CantidadParaConstruir;
        }  
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CantidadParaConstruir > 0)
        {
            Vector3 nuevaPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Construccion.transform.position = new Vector3(nuevaPos.x, nuevaPos.y, -0.2f);
            StartCoroutine("VerificarCuadriculaFondo");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones").GetComponent<DesplazamientoMapas>().NoMover = false;
        if (CantidadParaConstruir > 0)
        {
            if (_enCuadricula)
            {
                StopCoroutine("VerificarCuadriculaFondo");
                Vector3 posD = new Vector3(Construccion.transform.position.x, Construccion.transform.position.y, -0.2f);
                Vector3 posH = new Vector3(Construccion.transform.position.x, Construccion.transform.position.y, 0.1f);
                GolpesRays = new RaycastHit2D[10];
                Physics2D.Raycast(posD, posH, new ContactFilter2D(), GolpesRays);
                foreach (RaycastHit2D CObjeto in GolpesRays)
                {
                    if (CObjeto.collider != null)
                    {
                        if(AnchuraObjeto == 1 && CObjeto.transform.gameObject.name == nombresCuadricula[0])
                        {
                            string numeroN = "";
                            foreach (char letra in CObjeto.transform.gameObject.name)
                            {
                                if (char.IsNumber(letra))
                                {
                                    numeroN += letra;
                                }
                            }
                            MGM.NumeroCuadriculasBetadas.Add(numeroN);
                            Destroy(GameObject.Find(nombresCuadricula[0]).gameObject);
                            break;
                        }else if (AnchuraObjeto != 1 && CObjeto.transform.gameObject.name == nombresCuadricula[0])
                        {
                            string[] NombresTotales = new string[nombresCuadricula.Count];
                            foreach(string CNombre in nombresCuadricula)
                            {
                                    string numeroN = "";
                                    foreach (char letra in CNombre)
                                    {
                                        if (char.IsNumber(letra))
                                        {
                                            numeroN += letra;
                                        }
                                    }
                                    MGM.NumeroCuadriculasBetadas.Add(numeroN);
                                    Destroy(GameObject.Find(CNombre));
                            }
                            for(int n = 0; n < NombresTotales.Length; n++)
                            {
                                nombresCuadricula.Remove(NombresTotales[n]);
                            }
                            break;
                        }
                        
                    }
                }
                GameObject Temp = Instantiate(Resources.Load("Prefabs/Construcciones_Ciudad/Temporizador_Construcciones") as GameObject);
                Temp.GetComponent<TemporizadorConstrucciones>().IconoCambiarAlTerminar = IconoEdificio;
                Temp.GetComponent<TemporizadorConstrucciones>().TiempoParaActivar(FormatoTiempoConstruccion, TiempoConstruccion);
                Temp.GetComponent<TemporizadorConstrucciones>().NombreConstruccion = NombreEdificio + "_" + (CantidadParaConstruir);
                Construccion.AddComponent<CuartelGeneralConstruccionScript>();
                CantidadParaConstruir--;
                transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = CantidadParaConstruir.ToString();
                if (CantidadParaConstruir == 0)
                {
                    Color CColor = transform.GetChild(0).GetComponent<Image>().color;
                    transform.GetChild(0).GetComponent<Image>().color = new Color(CColor.r, CColor.g, CColor.b, CColor.a / 2);
                }
            }
            else
            {
                Destroy(Construccion);
            }
        }
    }

    IEnumerator VerificarCuadriculaFondo()
    {
        Vector3 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
        Vector3 posD = new Vector3(posMouse.x, posMouse.y, -0.2f);
        Vector3 posH = new Vector3(posMouse.x, posMouse.y, 0.1f);
        Physics2D.Raycast(posD,posH, new ContactFilter2D(), GolpesRays);
        foreach(RaycastHit2D CObjeto in GolpesRays)
        {
            if(CObjeto.collider != null && CObjeto.transform.gameObject.name.Contains("Cuadricula_Construcciones"))
            {
                bool Pasa = true;
                string NombreCuadriculaInicial = CObjeto.transform.gameObject.name;
                string numero1 = "";
                string numero2 = "";
                Vector3 PosPrimero = new Vector3();
                Vector3 PosFinal = new Vector3();

                foreach(char Letra in NombreCuadriculaInicial)
                {
                    if (char.IsNumber(Letra) && numero1 == "")
                    {
                        numero1 += Letra;
                    }else if (char.IsNumber(Letra) && numero2 == "")
                    {
                        numero2 += Letra;
                    }
                }
                nombresCuadricula = new List<string>();
                for (int n = 0 ; n < AnchuraObjeto; n++)
                {
                    int Anch = Convert.ToInt32(numero1);
                    for (int m = 0; m < AlturaObjeto; m++)
                    {
                        int Alt = Convert.ToInt32(numero2);
                        string nombreCuadriculas = "Cuadricula_Construcciones_" + (Anch - n) + "_" + (Alt + m);
                        if (GameObject.Find(nombreCuadriculas) == null)
                        {
                            Pasa = false;
                        }
                        else
                        {
                            nombresCuadricula.Add(nombreCuadriculas);

                            if (m == 0)
                            {
                                if (n + 1 == AnchuraObjeto)
                                {
                                    PosPrimero = GameObject.Find(nombreCuadriculas).transform.position;
                                }
                            }

                            if(m + 1 == AlturaObjeto)
                            {
                                if (n == 0)
                                {
                                    PosFinal = GameObject.Find(nombreCuadriculas).transform.position;
                                }
                            }

                           
                        }
                    }
                }

                if (Pasa)
                {
                    Construccion.transform.position = Vector3.Lerp(PosPrimero, PosFinal, 0.5f);
                    _enCuadricula = true;
                    break;
                }
                else
                {
                    _enCuadricula = false;
                    nombresCuadricula = null;
                }
            }
            else
            {        
                _enCuadricula = false;
                nombresCuadricula = null;
            }
        }
        if (nombresCuadricula == null)
        {
            Vector3 nuevaPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Construccion.transform.position = new Vector3(nuevaPos.x, nuevaPos.y, -0.2f);
        }
        GolpesRays = new RaycastHit2D[10];
        yield return new WaitForSeconds(.5f);
    }
}
