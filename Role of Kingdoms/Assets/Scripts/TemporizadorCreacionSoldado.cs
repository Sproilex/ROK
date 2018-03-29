using System.Collections;
using UnityEngine;

public class TemporizadorCreacionSoldado : MonoBehaviour {

    private float _TiempoTotal;
    private float _Contador;
    private GameObject _Construccion;
    private bool YaCreado;
    ManejadorGeneralMundo MGM;

    //Getters y Setters:

    public float TiempoTotal
    {
        get { return _TiempoTotal; }
        set { _TiempoTotal = value; }
    }

    public GameObject Construccion
    {
        get { return _Construccion; }
        set { _Construccion = value; }
    }

    //Metodos:

    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        _Contador = _TiempoTotal;
        StartCoroutine("PasarDatosAConstruccion");
    }

    IEnumerator PasarDatosAConstruccion()
    {
        _Contador -= 1;
        if (GameObject.Find(_Construccion.name) != null)
        {
            CuartelGeneralConstruccionScript CGCS = _Construccion.GetComponent<CuartelGeneralConstruccionScript>();
            if (_Contador <= 0)
            {
                CGCS.tiempoEnCero = true;
                if (!YaCreado)
                {
                    //Creacion de la informacion del soldado:
                    System.Random rnd = new System.Random();
                    System.Random rnd2 = new System.Random(rnd.Next(1, 56985698) * 96435576 / 14 * 8 / 52 * 89);
                    int numeroNombre = rnd2.Next(0, MGM.NombresDisponiblesSoldados.Count - 1);
                    string Nombre = MGM.NombresDisponiblesSoldados[numeroNombre];
                    int contador = Random.Range(0, MGM.RazasDisponibles.Count);
                    string Raza = "";
                    foreach (string CRaza in MGM.RazasDisponibles.Keys)
                    {
                        if(contador == 0)
                        {
                            Raza = CRaza;
                        }
                        contador--;
                    }
                    Sprite Icono = MGM.RazasDisponibles[Raza];
                    string Pasiva = "";
                    int contador2 = Random.Range(0, MGM.PasivasExistentes.Count);
                    foreach (string CLlave in MGM.PasivasExistentes.Keys)
                    {
                        if(contador2 == 0)
                        {
                            Pasiva = MGM.PasivasExistentes[CLlave].NombrePasiva;
                            break;
                        }
                        contador2--;
                    }

                    __informacionSoldados InfoSoldado = new __informacionSoldados(pDano: 1, pVida: 10, pHeroe: rnd.Next(0, 1) == 1,
                                                                                  pNombre: Nombre, pIcono: Icono,
                                                                                  pRaza: Raza, pClase: Pasiva);



                    MGM.CrearCartaSoldados(InfoSoldado);
                }
                Destroy(this.gameObject);
            }

            CGCS.Contador = _Contador;
        }
        else
        {
            if(_Contador <= 0 && !YaCreado)
            {
                //Creacion de la informacion del soldado:
                System.Random rnd = new System.Random();
                System.Random rnd2 = new System.Random(rnd.Next(1, 56985) * 58 * 2354 * 12065 / 7 * 6987 * 96435576);
                int numeroNombre = rnd2.Next(0, MGM.NombresDisponiblesSoldados.Count - 1);
                string Nombre = MGM.NombresDisponiblesSoldados[numeroNombre];
                int contador = Random.Range(0, MGM.RazasDisponibles.Count - 1);//rnd2.Next(0, MGM.RazasDisponibles.Count - 1);
                string Raza = "";
                foreach (string CRaza in MGM.RazasDisponibles.Keys)
                {
                    if (contador == 0)
                    {
                        Raza = CRaza;
                    }
                    contador--;
                }
                Sprite Icono = MGM.RazasDisponibles[Raza];
                string Pasiva = "";
                int contador2 = Random.Range(0, MGM.PasivasExistentes.Count - 1);//rnd2.Next(0, MGM.PasivasExistentes.Count - 1);
                foreach (string CLlave in MGM.PasivasExistentes.Keys)
                {
                    if (contador2 == 0)
                    {
                        Pasiva = MGM.PasivasExistentes[CLlave].NombrePasiva;
                        break;
                    }
                    contador2--;
                }

                __informacionSoldados InfoSoldado = new __informacionSoldados(pDano: 1, pVida: 10, pHeroe: rnd.Next(0, 1) == 1,
                                                                              pNombre: Nombre, pIcono: Icono,
                                                                              pRaza: Raza, pClase: Pasiva);



                MGM.CrearCartaSoldados(InfoSoldado);
                YaCreado = true;
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("PasarDatosAConstruccion");
    }

}
