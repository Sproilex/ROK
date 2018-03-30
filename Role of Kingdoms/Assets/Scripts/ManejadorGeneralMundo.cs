using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DocumentFormat.OpenXml.Packaging;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using DocumentFormat.OpenXml;
[Serializable]
public struct __informacionSoldados
{
    public bool EnMision;
    public int EXP;
    public int EXPAL;
    public int Daño;
    public int Vida;
    public int Energia;
    public int Poderes;
    public bool Heroe;
    public int Nivel;
    public bool Lesion;
    public string Nombre;
    public Sprite Icono;
    public string Clase;
    public string Raza;
    public bool MisionCompletada;
    public GameObject SoldadoEnLista;
    public GameObject SoldadoEnListaMision;
    private ManejadorGeneralMundo MGM;

    public __informacionSoldados(bool pEnMision = false,int pEXP = 0, int pEXPAL = 100,int pDano = 0,int pVida = 0,
                               int pEnergia = 100, int pPoderes = 0, bool pHeroe = false, int pNivel = 1,
                               bool pLesion = false, string pNombre = null, Sprite pIcono = null,
                               bool pMisionCompletada = true, string pClase = "",string pRaza = "",GameObject pSoldadoEnLista = null
                                ,GameObject pSoldadoEnListaMision = null)
    {
        EnMision = pEnMision;
        EXP = pEXP;
        EXPAL = pEXPAL;
        Daño = pDano;
        Vida = pVida;
        Energia = pEnergia;
        Poderes = pPoderes;
        Heroe = pHeroe;
        Nivel = pNivel;
        Lesion = pLesion;
        Nombre = pNombre;
        Icono = pIcono;
        Clase = pClase;
        Raza = pRaza;
        MisionCompletada = pMisionCompletada;
        SoldadoEnLista = pSoldadoEnLista;
        SoldadoEnListaMision = pSoldadoEnListaMision;
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
    }

    public void SubirNivel()
    {
        if(Nivel < MGM.LevelMaximoActual)
        {
            EXP -= EXPAL;
            Nivel += 1;
            Vida += 5;
            Daño += 1;
            EXPAL += 50;
            if (EXP >= EXPAL)
            {
                SubirNivel();
            }
        }
        else
        {
            EXP = EXPAL;
        }
        
    }

    public void ActualizarDatos(int EnergiaQuitar, int pEXP , bool pEnMision = true)
    {
        Energia -= EnergiaQuitar;
        EnMision = pEnMision;
        EXP += pEXP;
        if(EXP >= EXPAL)
        {
            SubirNivel();
        }
    }

    public void MostrarDatos()
    {
        Debug.Log("Nombre: " + Nombre);
        Debug.Log("EnMision: " + EnMision);
        Debug.Log("Mision Completada: " + MisionCompletada);
    }

    public static bool operator == (__informacionSoldados Obj1, __informacionSoldados Obj2)
    {
        if(Obj1.Nombre == Obj2.Nombre && Obj1.Nivel == Obj2.Nivel && Obj1.Raza == Obj2.Raza && Obj1.Clase == Obj2.Clase
           && Obj1.EXP == Obj2.EXP && Obj1.Heroe == Obj2.Heroe && Obj1.Vida == Obj2.Vida && Obj1.Icono == Obj2.Icono &&
           Obj1.Poderes == Obj2.Poderes)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator != (__informacionSoldados Obj1, __informacionSoldados Obj2)
    {
        if (Obj1.Nombre != Obj2.Nombre && Obj1.Nivel != Obj2.Nivel && Obj1.Raza != Obj2.Raza && Obj1.Clase != Obj2.Clase
           && Obj1.EXP != Obj2.EXP && Obj1.Heroe != Obj2.Heroe && Obj1.Vida != Obj2.Vida && Obj1.Icono != Obj2.Icono &&
           Obj1.Poderes != Obj2.Poderes)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return true;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

}
[Serializable]
public struct __informacionEnemigos
{
    public int Level;
    public string Nombre;
    public int Daño;
    public int Vida;
    public int Energia;
    public int Poderes;
    [NonSerialized]
    public Sprite Icono;

    public __informacionEnemigos(int pLevel = 0, string pNombre = "", int pDano = 0, int pVida = 0, int pEnergia = 0,
                                 int pPoderes = 0, Sprite pIcono = null)
    {
        Level = pLevel;
        Nombre = pNombre;
        Daño = pDano;
        Vida = pVida;
        Energia = pEnergia;
        Poderes = pPoderes;
        Icono = pIcono;
    }

    public void MostrarDatos()
    {
        Debug.Log("-----------");
        Debug.Log("Nombre: " + Nombre);
        Debug.Log("Level: " + Level);
        Debug.Log("Daño: " + Daño);
        Debug.Log("Vida: " + Vida);
        Debug.Log("Energia: " + Energia);
        Debug.Log("Poderes: " + Poderes);
        Debug.Log("-----------");
    }

}
[Serializable]
public struct __informacionPasivas
{
    public string NombrePasiva;
    public int Vida;
    public int Energia;
    public int Daño;

    public __informacionPasivas(string pNombre, int pVida, int pEnergia, int pDaño)
    {
        NombrePasiva = pNombre;
        Vida = pVida;
        Energia = pEnergia;
        Daño = pDaño;
    }

}

public enum EDificultadMisiones { Normal, PocoNormal, AlgoRaro, BastanteRaro, MuyRaro, UltraRaro, Extremo, MuyExtremo, UltraExtremo, CasiImposible,Nada};

public enum EFormatoTiempo { Minutos,Segundos,Horas,Dias};

public class ManejadorGeneralMundo : MonoBehaviour
{
    [Header("Informacion Respecto a Tropas:")]
    [Tooltip("Velocidad a la que se recarga la energia de las tropas")]
    public int VelocidadRecargaEnergiaSoldados;
    [Tooltip("Velocidad de movimiento de las tropas (Afecta el Tiempo de Misiones)")]
    public float VelocidadMovimientoTropas;
    [Tooltip("Duracion de la creacion de Soldados")]
    [Range(1,60)]
    public float TiempoGeneracionSoldados = 1;
    [Tooltip("Formato en que esta escrito el tiempo anterior")]
    public EFormatoTiempo FormatoTiempoGeneracionSoldados;
    [Tooltip("Level Maximo a la que los soldados pueden subir")]
    public int LevelMaximoActual;
    [Tooltip("Icono que aparecera en los soldados al moverse por el mapa")]
    public Sprite IconoSoldadosMovimiento;
    [Tooltip("Numero Maximo Inicial de Soldados que se pueden crear (Aumentara luego con la subida de nivel)")]
    public int NumeroMaximoSoldados;
    [Tooltip("Nombres Disponibles para Soldados")]
    [SerializeField]
    private List<string> _NombresDisponiblesSoldados = new List<string>();
    [SerializeField]
    public Dictionary<string, Sprite> RazasDisponibles = new Dictionary<string, Sprite>();
    [Tooltip("Lista de posibles voces que apareceran cuando los soldados sean de nivel muy bajo comparado a la mision")]
    public List<AudioClip> VocesAdvertenciaNivelNoAdmitido;

    private GameObject[] _MapasSinMisiones;
    private GameObject[] _MapasMisiones;
    private List<string> _NumeroCuadriculasBetadas = new List<string>();
    private int _NumeroSoldadosActuales;

    [SerializeField]
    public Dictionary<string,__informacionEnemigos> EnemigosExistentes = new Dictionary<string, __informacionEnemigos>();
    [SerializeField]
    public Dictionary<string, __informacionPasivas> PasivasExistentes = new Dictionary<string, __informacionPasivas>();

    private List<Mision> _MisionesEnMapa = new List<Mision>();

    [Header("Informacion sobre el Panel de Informacion de Misiones En Curso")]
    [Tooltip("Mensaje de Victoria que se muestra en el panel al ganar una mision")]
    public string MensajeVictoriaEnPanel;
    [Tooltip("Color del texto de Victoria que se muestra en el panel al ganar una mision")]
    public UnityEngine.Color ColorVictoria;
    [Tooltip("Mensaje de Derrota que se muestra en el panel al ganar una mision")]
    public string MensajeDerrotaEnPanel;
    [Tooltip("Color del texto de Derrota que se muestra en el panel al ganar una mision")]
    public UnityEngine.Color ColorDerrota;

    [SerializeField]
    public Dictionary<EDificultadMisiones, float> DificultadesDisponibles = new Dictionary<EDificultadMisiones, float>();

    [Header("Informacion Que se Necesita Añadir:")]
    [Tooltip("Si quiere que se creen 6 soldados virtuales para testear rapidamente")]
    public bool GenerarSoldadosDePrueba;
    [Range(2,36)]
    [Tooltip("Tamaño de la cuadricula de construccion del mapa de ciudad. El numero colocado estara elevado al cuadrado, por ejemplo, al ingresar 6 seria una cuadricula de 6x6")]
    public int TamanoCuadriculaConstrucciones;
    bool DatosCargados = false;
    public GraphicRaycaster GraphicRaycasterPrincipal;
    private ManejadorMisionPanel _MisionPanel;

    //Getters y Setters:

    public ManejadorMisionPanel MisionPanel
    {
        get { return _MisionPanel; }
        set { _MisionPanel = value; }
    }

    public List<string> NombresDisponiblesSoldados
    {
        get { return _NombresDisponiblesSoldados; }
        set { _NombresDisponiblesSoldados = value; }
    }

    public GameObject[] MapasSinMisiones
    {
        get
        {
            return _MapasSinMisiones;
        }
        set
        {
            _MapasSinMisiones = value;
        }
    }

    public GameObject[] MapasMisiones
    {
        get
        {
            return _MapasMisiones;
        }
        set
        {
            _MapasMisiones = value;
        }
    }

    public List<Mision> MisionesEnMapa
    {
        get { return _MisionesEnMapa; }
        set { _MisionesEnMapa = value; }
    }

    public int NumeroSoldadosActuales
    {
        get { return _NumeroSoldadosActuales; }
        set { _NumeroSoldadosActuales = value; }
    }

    public List<string> NumeroCuadriculasBetadas
    {
        get { return _NumeroCuadriculasBetadas; }
        set { _NumeroCuadriculasBetadas = value; }
    }

    //------------------------------------

    /// <summary>
    /// Colocacion de la informacion de los enemigos y los porcentajes a las diversas dificultades de misiones.
    /// </summary>
    public void CargarDatos()
    {
        if (_NombresDisponiblesSoldados.Count == 0 && RazasDisponibles.Count == 0 && PasivasExistentes.Count == 0 &&
            DificultadesDisponibles.Count == 0 && EnemigosExistentes.Count == 0)
        {
            DatosCargados = false;
        }

        if (!DatosCargados)
        {
            //Agregar Nombres posibles de Soldados desde su respectivo archivo excel
            LeerArchivoExcel(out _NombresDisponiblesSoldados, "NombreSoldados.xlsx");

            //Agregar Razas disponibles de Soldados desde su respectivo archivo excel
            List<string> InfoRazas = new List<string>();
            LeerArchivoExcel(out InfoRazas, "RazasSoldados.xlsx");
            int camposRecorridos = 0;
            string nombreRaza = "";
            for (int contador = 0; contador < InfoRazas.Count; contador++)
            {
                if (camposRecorridos == 0)
                {
                    nombreRaza = InfoRazas[contador];
                    camposRecorridos++;
                }
                else if (camposRecorridos == 1)
                {
                    Sprite Icono = null;
                    Sprite[] Iconos = (Resources.LoadAll<Sprite>(@"Iconos\Soldados\Atlas_razas_soldados"));
                    foreach (Sprite CIcono in Iconos)
                    {
                        if (CIcono.name == InfoRazas[contador])
                        {
                            Icono = CIcono;
                            break;
                        }
                    }
                    if (Icono != null)
                    {
                        RazasDisponibles.Add(nombreRaza, Icono);
                    }
                    else
                    {
                        Debug.LogError("Icono No Encontrado. Raza: " + nombreRaza + ". Verifica la escritura y ubicacion del mismo");
                    }

                    camposRecorridos = 0;
                }
            }
            //Agregar Pasivas disponibles desde su respectivo archivo excel
            List<string> InfoPasivas = new List<string>();
            LeerArchivoExcel(out InfoPasivas, "PasivasSoldados.xlsx");

            string NombrePasiva = "";
            int Vida = 0;
            int Energia = 0;
            int Dano = 0;
            int numeroCelda = 0;
            for (int contador = 0; contador < InfoPasivas.Count; contador++)
            {
                switch (numeroCelda)
                {
                    case 0:
                        NombrePasiva = InfoPasivas[contador];
                        numeroCelda++;
                        break;
                    case 1:
                        Vida = Convert.ToInt32(InfoPasivas[contador]);
                        numeroCelda++;
                        break;
                    case 2:
                        Energia = Convert.ToInt32(InfoPasivas[contador]);
                        numeroCelda++;
                        break;
                    case 3:
                        Dano = Convert.ToInt32(InfoPasivas[contador]);
                        __informacionPasivas InfoPasiva = new __informacionPasivas(NombrePasiva, Vida, Energia, Dano);
                        PasivasExistentes.Add(NombrePasiva, InfoPasiva);
                        numeroCelda = 0;
                        break;
                }
            }

            //Agregar Porcentajes para dificultades disponibles desde su respectivo archivo excel
            List<string> infoPorcentajes = new List<string>();
            LeerArchivoExcel(out infoPorcentajes, "PorcentajesDificultadesMisiones.xlsx");
            int NumeroCeldaPorcentajes = 0;
            string nombreDificultad = "";
            float porcentajeDificultad = 0.0f;
            for (int contador = 0; contador < infoPorcentajes.Count; contador++)
            {
                switch (NumeroCeldaPorcentajes)
                {
                    case 0:
                        nombreDificultad = infoPorcentajes[contador];
                        NumeroCeldaPorcentajes++;
                        break;
                    case 1:
                        porcentajeDificultad = Convert.ToSingle(infoPorcentajes[contador].Replace(".", ","));
                        EDificultadMisiones DificultadLista;
                        Enum.TryParse(nombreDificultad, out DificultadLista);
                        DificultadesDisponibles.Add(DificultadLista, porcentajeDificultad);
                        NumeroCeldaPorcentajes = 0;
                        break;
                }
            }

            ////Agregar Informacion de enemigos disponbiles desde su respectivo archivo excel
            List<string> infoEnemigos = new List<string>();
            LeerArchivoExcel(out infoEnemigos, "EnemigosInformacion.xlsx");
            int NumeroCeldaEnemigos = 0;
            __informacionEnemigos InfoCadaEnemigo = new __informacionEnemigos();
            for (int contador = 0; contador < infoEnemigos.Count; contador++)
            {
                switch (NumeroCeldaEnemigos)
                {
                    case 0:
                        InfoCadaEnemigo.Nombre = infoEnemigos[contador];
                        NumeroCeldaEnemigos++;
                        break;
                    case 1:
                        InfoCadaEnemigo.Level = Convert.ToInt32(infoEnemigos[contador]);
                        NumeroCeldaEnemigos++;
                        break;
                    case 2:
                        InfoCadaEnemigo.Daño = Convert.ToInt32(infoEnemigos[contador]);
                        NumeroCeldaEnemigos++;
                        break;
                    case 3:
                        InfoCadaEnemigo.Vida = Convert.ToInt32(infoEnemigos[contador]);
                        NumeroCeldaEnemigos++;
                        break;
                    case 4:
                        InfoCadaEnemigo.Energia = Convert.ToInt32(infoEnemigos[contador]);
                        NumeroCeldaEnemigos++;
                        break;
                    case 5:
                        InfoCadaEnemigo.Poderes = Convert.ToInt32(infoEnemigos[contador]);
                        NumeroCeldaEnemigos++;
                        break;
                    case 6:
                        Sprite[] IconosEnemigos = Resources.LoadAll<Sprite>(@"Iconos\Enemigos\enemigos_atlas_1");
                        foreach (Sprite CIcono in IconosEnemigos)
                        {
                            if (CIcono.name == infoEnemigos[contador])
                            {
                                InfoCadaEnemigo.Icono = CIcono;
                            }
                        }
                        EnemigosExistentes.Add(InfoCadaEnemigo.Nombre, InfoCadaEnemigo);
                        InfoCadaEnemigo = new __informacionEnemigos();
                        NumeroCeldaEnemigos = 0;
                        break;
                }
            }
            DatosCargados = true;
        }
    }

    /// <summary>
    /// Inicializacion de Variables y asignacion de valores por defecto a objetos de la escena
    /// </summary>
    void Start()
    {
        if (!DatosCargados)
        {
            CargarDatos();
        }

        GraphicRaycasterPrincipal = GameObject.Find("Interfaz").GetComponent<GraphicRaycaster>();
        Transform AIM = GameObject.FindGameObjectWithTag("Agrupador_Informacion_Misiones").transform;

        _MapasMisiones = GameObject.FindGameObjectsWithTag("Mapa");
        _MapasSinMisiones = GameObject.FindGameObjectsWithTag("Mapa_Sin_Misiones");

		GameObject[] Mapas = GameObject.FindGameObjectsWithTag ("Mapa");
		foreach(GameObject Cmapa in Mapas){
			if (Cmapa.name != "Mapa_Principal") {
				Cmapa.SetActive (false);
			}
		}
        GameObject[] Mapas2 = GameObject.FindGameObjectsWithTag("Mapa_Sin_Misiones");
        foreach (GameObject Cmapa2 in Mapas2)
        {
            Cmapa2.SetActive(false);
        }
        _MisionPanel = GameObject.FindGameObjectWithTag("Mision_Para_Enviar").GetComponent<ManejadorMisionPanel>();
        //Colocar valores por defecto a el panel de cuartel de soldados:
        Transform posPanelCreacionSoldados = GameObject.FindGameObjectWithTag("Interfaz_Panel_Soldados").transform.Find("Contenedor_Informacion_Creacion_Soldados");
        posPanelCreacionSoldados.Find("Panel_Cartas_Disponibles").Find("SubPanel_Cartas_Disponibles").Find("Texto_Cartas_Disponibles")
        .GetComponent<UnityEngine.UI.Text>().text = "0/" + NumeroMaximoSoldados;

        posPanelCreacionSoldados.Find("Panel_Cartas_Por_Crear").Find("SubPanel_Cartas_Por_Crear").Find("Texto_Cartas_Por_Crear")
        .GetComponent<UnityEngine.UI.Text>().text = NumeroMaximoSoldados + "/" + NumeroMaximoSoldados;

        //Colocar los valores por defecto a el panel de lista de soldados:
        Transform posListaSoldados = GameObject.FindGameObjectWithTag("Interfaz_Soldados").transform;
        posListaSoldados.Find("Visor_Seleccion_Cartas").Find("Cantidad_Soldados").GetComponent<UnityEngine.UI.Text>().text = "Soldados 0/" + NumeroMaximoSoldados;

        //Para Testear Rapidamente:
        if (GenerarSoldadosDePrueba)
        {
            Sprite[] Iconos = Resources.LoadAll<Sprite>("Iconos/Soldados/Atlas_razas_soldados");
            __informacionSoldados InfoSoldado = new __informacionSoldados(pDano: 1, pVida: 10, pHeroe: true,
                                                                                      pNombre: "Jose", pIcono: Iconos[2],
                                                                                      pRaza: "Gnomo", pClase: "Alquimista");

            __informacionSoldados InfoSoldado2 = new __informacionSoldados(pDano: 1, pVida: 10, pHeroe: false,
                                                                                      pNombre: "Anton", pIcono: Iconos[1],
                                                                                      pRaza: "Enano", pClase: "Pescador");

            __informacionSoldados InfoSoldado3 = new __informacionSoldados(pDano: 1, pVida: 10, pHeroe: false,
                                                                                      pNombre: "Mariano", pIcono: Iconos[3],
                                                                                      pRaza: "Humano", pClase: "Saqueador");
            __informacionSoldados InfoSoldado4 = new __informacionSoldados(pDano: 1, pVida: 10, pHeroe: false,
                                                                                      pNombre: "Malacath", pIcono: Iconos[2],
                                                                                      pRaza: "Gnomo", pClase: "Granjero");

            __informacionSoldados InfoSoldado5 = new __informacionSoldados(pDano: 1, pVida: 10, pHeroe: false,
                                                                                      pNombre: "Joselfas", pIcono: Iconos[0],
                                                                                      pRaza: "Elfo", pClase: "Observador");

            __informacionSoldados InfoSoldado6 = new __informacionSoldados(pDano: 1, pVida: 10, pHeroe: false,
                                                                                      pNombre: "Ark", pIcono: Iconos[0],
                                                                                      pRaza: "Elfo", pClase: "Archivador");

            CrearCartaSoldados(InfoSoldado);
            CrearCartaSoldados(InfoSoldado2);
            CrearCartaSoldados(InfoSoldado3);
            CrearCartaSoldados(InfoSoldado4);
            CrearCartaSoldados(InfoSoldado5);
            CrearCartaSoldados(InfoSoldado6);
        }
        
    }

    /// <summary>
    /// Activar un mapa y desactivar a los demas
    /// </summary>
    /// <param name="NombreMapa"></param>
    public void CambiarMapa(string NombreMapa)
    {
        //foreach (GameObject Cmapa in MapasMisiones)
        //{
        //    if(Cmapa.name == NombreMapa)
        //    {
        //        Cmapa.SetActive(true);
        //        GameObject.Find("Boton_Abrir_Construcciones_Ciudad").GetComponent<Button>().interactable = false;
        //    }
        //    else
        //    {
        //        Cmapa.SetActive(false);
        //    }
        //}
        //foreach (GameObject Cmapa in MapasSinMisiones)
        //{
        //    if (Cmapa.name == NombreMapa)
        //    {
        //        Cmapa.SetActive(true);
        //        GameObject.Find("Boton_Abrir_Construcciones_Ciudad").GetComponent<Button>().interactable = true;
        //    }
        //    else
        //    {
        //        Cmapa.SetActive(false);
        //    }
        //}
        GameObject MisionesM = MapasMisiones[0];
        GameObject CiudadM = MapasSinMisiones[0];
        MisionesM.SetActive(MisionesM.name == NombreMapa);
        CiudadM.SetActive(CiudadM.name == NombreMapa);
        GameObject.Find("Boton_Abrir_Construcciones_Ciudad").GetComponent<Button>().interactable = CiudadM.activeSelf;
        OcultarDesplegarBotonesSegunMapas();
    }

    /// <summary>
    /// Activar y Desactivar objetos seleccionables del mapa (Filtro, misiones, boton del filtro y botones de lugares)
    /// </summary>
    /// <param name="EDBE">Si fue llamado desde el Boton de Enviar (Si ya se enviaron los soldados o no)</param>
    /// <param name="DesactivarMAM">Si se quiere Desactivar la Mision Actual Mostrada</param>
    public void ActivarDesactivarSeleccionablesDelMapa(bool EDBE,bool DesactivarMAM = true,bool Activar = false,bool DesactivarMovMapas = true)
    {
        //Activar/Desactivar seleccion del boton de soldados:
        Transform PosListaSoldados = GameObject.Find("Boton_Desplegar_Panel_Soldados").transform;
        PosListaSoldados.GetComponent<Button>().interactable = Activar;
        GameObject.FindGameObjectWithTag("Interfaz_Soldados").GetComponent<ManejadorListaSoldados>().DesplegarOcultarPanel(true);

        //Plegar Interfaz de Creacion de Soldados:
        GameObject InterfazCreacionSoldados = GameObject.FindGameObjectWithTag("Interfaz_Panel_Soldados");
        InterfazCreacionSoldados.GetComponent<ManejadorPanelInformacionCuartelSoldados>().DesplegarOcultarPanel(true);
        GameObject.Find("Boton_Desplegar_Panel_Soldados").GetComponent<Button>().interactable = Activar;

        //Desactivar/Activar Seleccion de Misiones:
        for (int contador = 0; contador < MisionesEnMapa.Count; contador++)
        {
            GameObject COM = GameObject.Find(MisionesEnMapa[contador].MisionOriginal);

            if (COM != null && !COM.GetComponent<Mision>().SoldadoEnCamino)
            {
                COM.GetComponent<BoxCollider2D>().enabled = Activar;
            }
        }

        //Desactivar/Activar filtro:
        FindObjectOfType<FiltroMisiones>().DesplegarOcultarPanel(true);
        GameObject.Find("Boton_Abrir_Panel_Filtros").GetComponent<Button>().interactable = Activar;

        //Botones de Cambios de Mapas
        Transform PosPL = GameObject.Find("Panel_Pestana_Lugares").transform;
        for (int n = 0; n < PosPL.childCount; n++)
        {
            PosPL.GetChild(n).GetComponent<Button>().interactable = Activar;
        }

        //Desactivar/Activar boton de abrir panel de informacion de misiones:
        GameObject.Find("Boton_Abrir_Panel_Informacion_Misiones").GetComponent<Button>().interactable = Activar;

        //Desactivar Boton de Construcciones
        if (GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones") != null)
        {
            GameObject.Find("Boton_Abrir_Construcciones_Ciudad").GetComponent<Button>().interactable = Activar;
        }

        //Desactivar/Activar Movimiento del mapa si se quiere:
        if (DesactivarMovMapas)
        {
            DesplazamientoMapas[] MovMapas = GameObject.FindObjectsOfType<DesplazamientoMapas>();
            foreach(DesplazamientoMapas CMapa in MovMapas)
            {
                CMapa.GetComponent<BoxCollider2D>().enabled = Activar;
            }
        }

        //Desactivar/Activar Seleccion de Construcciones si esta la ciudad activa:
        GameObject Ciudad = GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones");

        if(Ciudad != null)
        {
            EdificioCiudad[] Construcciones = FindObjectsOfType<EdificioCiudad>();
            foreach(EdificioCiudad CConstruccion in Construcciones)
            {
                BoxCollider2D CCollider = CConstruccion.GetComponent<BoxCollider2D>();
                if (CCollider != null && CConstruccion.Activo)
                {
                    CCollider.enabled = Activar;
                }
            }
        }

        //Desactivar/Activar Panel de informacion de misiones:
        GameObject.FindObjectOfType<ManejadorPanelInformacionMisiones>().OcultarDesplegarPanel(true);

        if (DesactivarMAM)
        {
            CerrarYLimpiarMisionMostrada(EDBE);
        }

    }

    /// <summary>
    /// Cierra la Mision Actual Mostrada y Limpia la informacion de los soldados de ella.
    /// </summary>
    /// <param name="DesbloquearSoldados">Si se quieren desbloquear los soldados o no</param>
    /// <param name="nombreMision">El nombre de la mision que se quiere eliminar</param>
    public void CerrarYLimpiarMisionMostrada(bool DesbloquearSoldados)
    {
        Mision ScriptMM = GameObject.Find(_MisionPanel.NombreMisionMapa).GetComponent<Mision>();
        if (DesbloquearSoldados)
        {
            for(int n = 0; n < ScriptMM.EspacioTropas.Length; n++)
            {
                GameObject objSoldado = ScriptMM.infoSoldadosDisponibles[n].SoldadoEnLista;
                GameObject objSoldadoM = ScriptMM.infoSoldadosDisponibles[n].SoldadoEnListaMision;
                Soldados Soldado = objSoldado != null ? objSoldado.GetComponent<Soldados>() : null;
                Soldados SoldadoM = objSoldadoM != null ? objSoldadoM.GetComponent<Soldados>() : null;
                if (Soldado != null && Soldado.InfoActualSoldado.MisionCompletada)
                {
                    Soldado.ActivarTropa();
                    SoldadoM.ActivarTropa();
                    SoldadoM.PosibleEnviarAMision = true;
                    ScriptMM.infoSoldadosDisponibles[n] = new __informacionSoldados();
                    ScriptMM.EspacioTropas[n] = false;
                }
            }
        }
        _MisionPanel.DesplegarOcultarPanel();
    }

    public void CerrarYLimpiarMisionMostradaBoton(bool DesbloquearSoldados)
    {
        CerrarYLimpiarMisionMostrada(DesbloquearSoldados);
        ActivarDesactivarSeleccionablesDelMapa(false,false,true);
    }

    /// <summary>
    /// Desactiva o Activa la posibilidad de desplazarse por el mapa
    /// </summary>
    /// <param name="nombreMapa">Nombre del mapa a desactivar/activar</param>
    /// <param name="activar">Si se quiere activar o desactivar</param>
    public void ActivarDesactivarMovimientoMapa(string nombreMapa,bool activar = true)
    {
        GameObject.Find(nombreMapa).GetComponent<BoxCollider2D>().enabled = activar;
    }

    /// <summary>
    /// Crea una nueva carta de un soldado en la lista y en la informacion de la mision (ambos conectados como uno)
    /// </summary>
    /// <param name="infoNueva">Informacion a utilizar para crear el soldado</param>
    public void CrearCartaSoldados(__informacionSoldados infoNueva)
    {
        int numeroActualSoldados = NumeroSoldadosActuales + 1;
        GameObject prefabCarta = Resources.Load("Prefabs/Soldados/PrefabCartasSoldados") as GameObject;
        Transform posSoldadosLista = GameObject.FindGameObjectWithTag("Soldados_Listados").transform;
        Transform posSoldadosListaMision = GameObject.Find("Panel_Lista_Tropas").transform;
        Transform PosInstanciarLista = numeroActualSoldados <= 5 ? posSoldadosLista.Find("Espacio_Soldado_" + numeroActualSoldados) :
                                                                   posSoldadosLista.parent.Find("Espacio_Ocultos");

        Transform PosInstanciarMisiones = numeroActualSoldados <= 5 ? posSoldadosListaMision.Find("Borde_Seleccion_Carta_" + numeroActualSoldados) :
                                                                      posSoldadosListaMision.Find("Espacio_Ocultos");

        GameObject CartaEnLista = Instantiate(prefabCarta, PosInstanciarLista);
        GameObject CartaEnMisiones = Instantiate(prefabCarta, PosInstanciarMisiones);
        CartaEnLista.name = "Carta_Soldado_" + numeroActualSoldados;
        CartaEnMisiones.name = "Carta_Soldado_" + numeroActualSoldados;
        __informacionSoldados InfoActualizada = infoNueva;
        InfoActualizada.SoldadoEnLista = CartaEnLista;
        InfoActualizada.SoldadoEnListaMision = CartaEnMisiones;
        CartaEnLista.GetComponent<Soldados>().InfoActualSoldado = InfoActualizada;
        CartaEnLista.GetComponent<Soldados>().ActualizarInformacionFisicamente();
        CartaEnMisiones.GetComponent<Soldados>().InfoActualSoldado = InfoActualizada;
        CartaEnMisiones.GetComponent<Soldados>().PosibleEnviarAMision = true;
        CartaEnMisiones.GetComponent<Soldados>().ActualizarInformacionFisicamente();
        Transform posSoldados = GameObject.FindGameObjectWithTag("Interfaz_Soldados").transform.Find("Visor_Seleccion_Cartas");
        posSoldados.Find("Cantidad_Soldados").GetComponent<UnityEngine.UI.Text>().text = numeroActualSoldados + "/" + NumeroMaximoSoldados;
        NumeroSoldadosActuales = numeroActualSoldados;
        DesplazamientoListadoTropasMision[] STropas = GameObject.FindObjectsOfType<DesplazamientoListadoTropasMision>();
        int numeroSoldados = 0;
        foreach (DesplazamientoListadoTropasMision CStropas in STropas)
        {
            CStropas.Soldados.Add(CartaEnLista);
            CStropas.SoldadosMision.Add(CartaEnMisiones);
            numeroSoldados = CStropas.Soldados.Count;
        }
        if(numeroSoldados > 5)
        {
            GameObject.FindGameObjectWithTag("Soldados_Listados").transform.parent.Find("Flecha_Derecha").GetComponent<Button>().interactable = true;
            GameObject.FindGameObjectWithTag("Mision_Para_Enviar").transform.Find("Detalles_Mision").Find("Flecha_Derecha").GetComponent<Button>().interactable = true;
        }
    }

    /// <summary>
    /// Oculta botones que no se utilizaran en el mapa activo actualmente
    /// </summary>
    public void OcultarDesplegarBotonesSegunMapas()
    {
        bool CiudadActiva = GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones");

        Animator BotonListaSoldados = GameObject.Find("Boton_Desplegar_Panel_Soldados").GetComponent<Animator>();
        BotonListaSoldados.SetBool("Arriba", !CiudadActiva);
        BotonListaSoldados.SetBool("Abajo", CiudadActiva);

        Animator BotonPanelInformacionMisiones = GameObject.Find("Boton_Abrir_Panel_Informacion_Misiones").GetComponent<Animator>();
        BotonPanelInformacionMisiones.SetBool("Arriba", !CiudadActiva);
        BotonPanelInformacionMisiones.SetBool("Abajo", CiudadActiva);

        Animator BotonFiltro = GameObject.Find("Boton_Abrir_Panel_Filtros").GetComponent<Animator>();
        BotonFiltro.SetBool("Izquierda", !CiudadActiva);
        BotonFiltro.SetBool("Derecha", CiudadActiva);

        Animator BotonConstrucciones = GameObject.Find("Boton_Abrir_Construcciones_Ciudad").GetComponent<Animator>();
        BotonConstrucciones.SetBool("Izquierda", !CiudadActiva);
        BotonConstrucciones.SetBool("Derecha", CiudadActiva);
    }

    /// <summary>
    /// Cambia el valor de un tiempo del formato que tenga (Horas, dias, minutos,segundos) a segundos
    /// </summary>
    /// <param name="FormatoTiempo">En que formato esta representado el tiempo</param>
    /// <param name="TiempoACambiar">La cantidad o valor del tiempo que se quiere cambiar</param>
    /// <returns></returns>
    public static float CambiarTiempoAFormato(EFormatoTiempo FormatoTiempo, float TiempoACambiar)
    {
        int NumeroMultiplicar = 0;

        switch (FormatoTiempo)
        {
            case EFormatoTiempo.Minutos:
                NumeroMultiplicar = 60;
                break;
            case EFormatoTiempo.Horas:
                NumeroMultiplicar = 3600;
                break;
            case EFormatoTiempo.Dias:
                NumeroMultiplicar = 86400;
                break;
            case EFormatoTiempo.Segundos:
                NumeroMultiplicar = 1;
                break;
        }

        return TiempoACambiar * NumeroMultiplicar;
            


    }

    void LeerArchivoExcel(out List<string> Datos,string nombreArchivo)
    {
        SpreadsheetDocument SSD = null;
        if (File.Exists(Directory.GetCurrentDirectory() + @"\Assets\Informacion en Excel\" + nombreArchivo))
        {
            SSD = SpreadsheetDocument.Open(Directory.GetCurrentDirectory() + @"\Assets\Informacion en Excel\" + nombreArchivo, true);
        }else if (File.Exists(Directory.GetCurrentDirectory() + @"\" + nombreArchivo))
        {
            SSD = SpreadsheetDocument.Open(Directory.GetCurrentDirectory() + @"\" + nombreArchivo, true);
        }
        else
        {
            Debug.LogError("No se ha encontrado el archivo '" + nombreArchivo + "'. Verifique la direccion o el nombre");
            Datos = null;
            return;
        }
        WorkbookPart WBP = SSD.WorkbookPart;
        WorksheetPart WSP = WBP.WorksheetParts.First();
        var stringTable = WBP.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
        OpenXmlReader lector = OpenXmlReader.Create(WSP);
        List<string> DatosListos = new List<string>();
        int contador = 0;
        while (lector.Read())
        {

            if(lector.ElementType == typeof(Row))
            {
                contador++;
            }

            if (lector.ElementType == typeof(Cell) && contador > 1)
            {
                Cell Celda = (Cell)lector.LoadCurrentElement();
                string valor = null;

                if (Celda != null)
                {
                    valor = Celda.InnerText;
                    if (Celda.DataType != null && stringTable != null)
                    {
                        string Texto = stringTable.SharedStringTable.ElementAt(int.Parse(valor)).InnerText;
                        if (!Texto.Contains("//"))
                            DatosListos.Add(Texto);
                    }
                    else if(Celda.DataType == null && valor != "")
                    {
                        DatosListos.Add(valor);
                    }
                }
            }
        }
        Datos = DatosListos;
    }

}