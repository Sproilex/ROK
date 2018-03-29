using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Mision : MonoBehaviour
{
    private ManejadorGeneralMundo MGM;
    [Range(1,3)]
    public bool[] EspacioTropas = new bool[1];
    //Informacion de los Soldados:
    public __informacionSoldados[] infoSoldadosDisponibles;
    //Información Enemigos:
    public string[] NombresEnemigos = new string[1];
    private __informacionEnemigos[] InfoEnemigos;
    private int _levelPromedio = 0;
    //Información de Batalla:
    public EDificultadMisiones DificultadMision;
    public bool EsMisionUnica;
    public string NombreBatalla;
    public string DescripcionBatalla;

    public int TiempoParaRespawn;
    public EFormatoTiempo FormatoTiempoParaRespawn;

    public int TiempoEnMapaBatalla;
    private float ContadorTiempoEnMapaBatalla;
    public EFormatoTiempo FormatoTiempoEnMapaBatalla;

    public int TiempoPeleaBatalla;
    public EFormatoTiempo FormatoTiempoPeleaBatalla;
    private int _ProbabilidadExitoJugador = 0;

    public int CantidadMinimaEnergiaRestarSoldados;

    public int CantidadMaximaEnergiaRestarSoldados;

    public int EXPADarASoldados;
    public Sprite IconoMision;
    private float _tiempoFinal;
    private bool _SoldadoEnCamino;
    private string _MisionOriginal;
    private SpawnMisiones _ManejadorDeSpawn;
    private GameObject _PrefabMision;
    public bool EsInformacion;
    private GameObject Temporizador;

    List<RaycastResult> _resultadoRaycast = new List<RaycastResult>();
    PointerEventData _punteroInfo = new PointerEventData(null);
    GraphicRaycaster _GR;

    //Getters y Setters:

    public SpawnMisiones ManejadorDeSpawn
    {
        get { return _ManejadorDeSpawn; }
        set { _ManejadorDeSpawn = value; }
    }

    public GameObject PrefabMision
    {
        get { return _PrefabMision; }
        set { _PrefabMision = value; }
    }

    public string MisionOriginal
    {
        get { return _MisionOriginal; }
        set { _MisionOriginal = value; }
    }

    public int LevelPromedio
    {
        get { return _levelPromedio; } set { _levelPromedio = value; }
    }

    public int ProbabilidadExitoJugador
    {
        get
        {
            return _ProbabilidadExitoJugador;
        }

        set
        {
            _ProbabilidadExitoJugador = value;
        }
    }

    public float TiempoFinal
    {
        get { return _tiempoFinal; }
        set { _tiempoFinal = value; }
    }

    public bool SoldadoEnCamino
    {
        get { return _SoldadoEnCamino; }
        set { _SoldadoEnCamino = value; }
    }

    //----------------------------------------------

    /// <summary>
    /// Inicializacion de Variables, colocar info de los enemigos y actualizar 
    /// la info de los paneles de informacion al lado de la mision
    /// </summary>
	void Start(){
        MGM = GameObject.Find("ManejadorGeneralMundo").GetComponent<ManejadorGeneralMundo>();
        infoSoldadosDisponibles = new __informacionSoldados[EspacioTropas.Length];


        ContadorTiempoEnMapaBatalla = ManejadorGeneralMundo.CambiarTiempoAFormato(FormatoTiempoEnMapaBatalla, TiempoEnMapaBatalla);

        InfoEnemigos = new __informacionEnemigos[NombresEnemigos.Length];
        _levelPromedio = 0;
        for (int contador = 0; contador < InfoEnemigos.Length; contador++)
        {
                MGM.EnemigosExistentes.TryGetValue(NombresEnemigos[contador], out InfoEnemigos[contador]);
                _levelPromedio += InfoEnemigos[contador].Level;
        }
        _levelPromedio = _levelPromedio / InfoEnemigos.Length;

        if (this.transform.childCount != 0)
        {
            this.transform.Find ("Level").GetComponent<TextMesh> ().text = _levelPromedio.ToString();
		}

        if (!EsInformacion && Temporizador == null)
        {
            Temporizador = new GameObject();
            Temporizador.name = "[TemporizadorTiempoEnMapa]";
            Temporizador.AddComponent(typeof(TemporizadorTiempoEnMapaMisiones));
            Temporizador.GetComponent<TemporizadorTiempoEnMapaMisiones>().Mision = this;
        }
        _GR = MGM.GraphicRaycasterPrincipal;

    }

    /// <summary>
    /// LLama al metodo de mostrar la Info de la Mision en Pantalla
    /// </summary>
    void OnMouseDown()
    {
        _punteroInfo.position = Input.mousePosition;
        _GR.Raycast(_punteroInfo, _resultadoRaycast);

        if (_resultadoRaycast.Count == 0)
        {
            MostrarLaInfoMision();
        }
    }

    /// <summary>
    /// Eliminar Soldado de la informacion de la mision
    /// </summary>
    /// <param name="NumeroSoldado">Numero de soldado a eliminar</param>
    public void EliminarSoldado(int NumeroSoldado)
    {
        infoSoldadosDisponibles[NumeroSoldado] = new __informacionSoldados();
        EspacioTropas[NumeroSoldado] = false;
    }

    /// <summary>
    /// Actualiza virtualmente los resultados de la probabilidad de exito del jugador
    /// </summary>
    public void ActualizarResultados()
    {
        decimal ResultadoTropa = 0;
        decimal ResultadoEnemigo = 0;
        float ResultadoTotal1 = 0;


        //Sacar el resultado de la suma de los atributos de las tropas y del enemigo.
        for (int contador = 0; contador < EspacioTropas.Length; contador++)
        {
            if (EspacioTropas[contador])
            {
                ResultadoTropa = ResultadoTropa + ((infoSoldadosDisponibles[contador].Daño + infoSoldadosDisponibles[contador].Vida) * infoSoldadosDisponibles[contador].Energia);
            }
        }

        for(int contador = 0; contador < InfoEnemigos.Length; contador++)
        {
            ResultadoEnemigo += (InfoEnemigos[contador].Daño + InfoEnemigos[contador].Vida) * InfoEnemigos[contador].Energia;
        }
        ResultadoTotal1 = (float)System.Decimal.Divide(ResultadoTropa * 100,ResultadoEnemigo);

        int PorcentajeResultado = (int)(Mathf.Round(ResultadoTotal1) - 100);

        if (ResultadoTropa > ResultadoEnemigo)
        {
            ProbabilidadExitoJugador = 50 + PorcentajeResultado;
        }
        else
        {
            ProbabilidadExitoJugador = 50 - Mathf.Abs(PorcentajeResultado);
        }

        ProbabilidadExitoJugador = ProbabilidadExitoJugador > 100 ? 100 : ProbabilidadExitoJugador;

        ProbabilidadExitoJugador = ProbabilidadExitoJugador < 0 ? 0 : ProbabilidadExitoJugador;

        CambiarProbabilidadGanar();
    }

    /// <summary>
    /// Actualiza fisicamente los resultados de la probabilidad de exito del jugador
    /// </summary>
    public void CambiarProbabilidadGanar()
    {
        Transform Pos = GameObject.FindGameObjectWithTag("Mision_Para_Enviar").transform.
                        Find("Detalles_Mision").Find("Panel_Informacion_Y_Enemigos").Find("Elementos_Mision");
        Text TextoPorcentaje = Pos.Find("Probabilidad_Victoria").GetComponent<Text>();
        TextoPorcentaje.text = _ProbabilidadExitoJugador.ToString() + " % victoria";
    }

    /// <summary>
    /// Muestra la info de la mision en el panel correspondiente.
    /// </summary>
    /// <param name="CerrarMisionAnterior">Si se quiere cerrar una mision previamente abierta</param>
    public void MostrarLaInfoMision(bool CerrarMisionAnterior = false)
    {
        Transform InfoMisionPosicion = GameObject.FindGameObjectWithTag("Mision_Para_Enviar").transform;

        ManejadorMisionPanel InfoMision = InfoMisionPosicion.GetComponent<ManejadorMisionPanel>();
        //InfoMision.resetearDatos();
        GameObject.FindObjectOfType<ManejadorDetallesPanelMision>().ResetearDatos();

        //Colocar Datos en el Encabezado:
        Transform EncabezadoPosicion = InfoMisionPosicion.Find("Encabezado_Mision").Find("Panel_Encabezado_Texto_Mision");
        EncabezadoPosicion.Find("Texto_Nombre_Batalla").GetComponent<Text>().text = NombreBatalla;
        EncabezadoPosicion.Find("Texto_Descripcion_Batalla").GetComponent<Text>().text = '"' + DescripcionBatalla + '"';

        //Colocar Datos en los Elementos de la Mision:
        Transform ElementosMisionPosicion = InfoMisionPosicion.Find("Detalles_Mision").Find("Panel_Informacion_Y_Enemigos").Find("Elementos_Mision");
        if (EspacioTropas.Length < 3)
        {
            for (int contador = 1; contador <= (3 - EspacioTropas.Length); contador++)
            {
                ElementosMisionPosicion.Find("Espacio_Soldado_" + (EspacioTropas.Length + contador)).gameObject.SetActive(false);
            }
        }

        for (int contador = 0; contador < NombresEnemigos.Length; contador++)
        {
            Transform PosEnemigo = ElementosMisionPosicion.Find("Enemigo_" + (contador + 1));
            PosEnemigo.Find("Level").GetComponent<Text>().text = InfoEnemigos[contador].Level.ToString();
            PosEnemigo.Find("Icono").GetComponent<Image>().sprite = InfoEnemigos[contador].Icono;
        }

        //Eliminar espacios de enemigos y tropas que sobran
        if (NombresEnemigos.Length < 3)
        {

            for (int contador = 1; contador <= (3 - NombresEnemigos.Length); contador++)
            {
                ElementosMisionPosicion.Find("Enemigo_" + (NombresEnemigos.Length + contador)).gameObject.SetActive(false);
            }
        }


        //Colocar Datos en Panel de Duracion, Descripcion y Enviar:
        Transform PanelDDE = ElementosMisionPosicion.Find("Panel_Duracion_Descripcion_Enviar");
        string textoPonerDuracion = "Finaliza en : ";
        Transform CEM = GameObject.FindGameObjectWithTag("Ciudad_En_Mapa").transform;
        Vector3 posf = transform.localPosition - CEM.localPosition;
        _tiempoFinal = posf.magnitude * 100 / (MGM.VelocidadMovimientoTropas);
        _tiempoFinal += ManejadorGeneralMundo.CambiarTiempoAFormato(FormatoTiempoPeleaBatalla,TiempoPeleaBatalla);

        if (_tiempoFinal > 59 && _tiempoFinal <= 3599)
        {
            float tiempoListo = Mathf.Round(_tiempoFinal / 60);
            textoPonerDuracion += tiempoListo + (tiempoListo != 1 ? " Minutos" : " Minuto");
        }
        else if (_tiempoFinal > 3599)
        {
            float tiempoListo = Mathf.Round(_tiempoFinal / 3600);
            textoPonerDuracion += tiempoListo + (tiempoListo != 1 ? " Horas" : " Hora");
        }
        else
        {
            float tiempoListo = Mathf.Round(_tiempoFinal);
            textoPonerDuracion += tiempoListo + (tiempoListo != 1 ? " Segundos" : " Segundo");
        }

        PanelDDE.Find("Texto_Duracion").GetComponent<Text>().text = textoPonerDuracion;
        PanelDDE.Find("Texto_Recompensa_EXP").GetComponent<Text>().text = "+ " + EXPADarASoldados + " puntos XP";

        int NivelMinimo = InfoEnemigos[0].Level;
        
        for (int contador2 = 0; contador2 < NombresEnemigos.Length; contador2++)
        {
            if (InfoEnemigos[contador2].Level < NivelMinimo)
            {
                NivelMinimo = InfoEnemigos[contador2].Level;
            }
        }
        NivelMinimo -= 2;
        List<Soldados> SoldadosMision = new List<Soldados>();
        Transform PosCartas = InfoMision.transform.Find("Detalles_Mision").Find("Panel_Lista_Tropas");
        
        for(int contador = 1; contador <= 5; contador++)
        {
            GameObject ObjetoSoldado = null;
            if (PosCartas.Find("Borde_Seleccion_Carta_" + contador).childCount > 0)
                ObjetoSoldado = PosCartas.Find("Borde_Seleccion_Carta_" + contador).GetChild(0).gameObject;
            if(ObjetoSoldado != null)
            {
                SoldadosMision.Add(ObjetoSoldado.GetComponent<Soldados>());
            }
        }

        for (int contador = 0; contador < PosCartas.Find("Espacio_Ocultos").childCount; contador++)
        {
            GameObject ObjetoSoldado = PosCartas.Find("Espacio_Ocultos").GetChild(contador).gameObject;
            if (ObjetoSoldado != null)
            {
                SoldadosMision.Add(ObjetoSoldado.GetComponent<Soldados>());
            }
        }

        foreach(Soldados CSoldado in SoldadosMision)
        {
            if(CSoldado.InfoActualSoldado.Nivel < NivelMinimo)
            {
                CSoldado.GetComponent<Image>().color = new Color(1,0,0,1);
                CSoldado.NoEsNivelMision = true;
            }
            else
            {
                CSoldado.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                CSoldado.NoEsNivelMision = false;
            }
        }

        InfoMision.NombreMisionMapa = this.name;
        MGM.ActivarDesactivarSeleccionablesDelMapa(false,false);
        InfoMision.DesplegarOcultarPanel();
    }

    /// <summary>
    /// Actualizar informacion de la mision por una nueva
    /// </summary>
    /// <param name="infoNueva">Informacion nueva por la que sera reemplazada la anterior</param>
    public void ActualizarInformacion(Mision infoNueva)
    {
        EsMisionUnica = infoNueva.EsMisionUnica;
        DificultadMision = infoNueva.DificultadMision;
        _PrefabMision = infoNueva._PrefabMision;
        _ManejadorDeSpawn = infoNueva._ManejadorDeSpawn;
        EspacioTropas = infoNueva.EspacioTropas;
        infoSoldadosDisponibles = infoNueva.infoSoldadosDisponibles;
        NombresEnemigos = infoNueva.NombresEnemigos;
        InfoEnemigos = infoNueva.InfoEnemigos;
        TiempoEnMapaBatalla = infoNueva.TiempoEnMapaBatalla;
        FormatoTiempoEnMapaBatalla = infoNueva.FormatoTiempoEnMapaBatalla;
        LevelPromedio = infoNueva.LevelPromedio;
        NombreBatalla = infoNueva.NombreBatalla;
        TiempoParaRespawn = infoNueva.TiempoParaRespawn;
        FormatoTiempoParaRespawn = infoNueva.FormatoTiempoParaRespawn;
        TiempoPeleaBatalla = infoNueva.TiempoPeleaBatalla;
        FormatoTiempoPeleaBatalla = infoNueva.FormatoTiempoPeleaBatalla;
        CantidadMinimaEnergiaRestarSoldados = infoNueva.CantidadMinimaEnergiaRestarSoldados;
        CantidadMaximaEnergiaRestarSoldados = infoNueva.CantidadMaximaEnergiaRestarSoldados;
        EXPADarASoldados = infoNueva.EXPADarASoldados;
        IconoMision = infoNueva.IconoMision;
        if(this.GetComponent<SpriteRenderer>() != null)
        {
            this.transform.Find("Icono_Mision").GetComponent<SpriteRenderer>().sprite = IconoMision;
        }
        TiempoFinal = infoNueva.TiempoFinal;
        SoldadoEnCamino = infoNueva.SoldadoEnCamino;
        MisionOriginal = infoNueva.MisionOriginal;
        DescripcionBatalla = infoNueva.DescripcionBatalla;
        Start();
    }

    /// <summary>
    /// Muestra la informacion del enemigo en el subpanel de informacion del panel de enviar a misiones.
    /// </summary>
    /// <param name="numeroEnemigo">Cual de los 3 enemigos es el que se quiere mostrar</param>
    public void MostrarInformacionEnemigo(int numeroEnemigo)
    {
        ManejadorDetallesPanelMision MDPM = GameObject.FindObjectOfType<ManejadorDetallesPanelMision>();
        MDPM.CambiarDatos(InfoEnemigos[numeroEnemigo - 1]);
    }

    public IEnumerator VerificarTiempoEnMapa()
    {
        ContadorTiempoEnMapaBatalla -= Time.deltaTime;
        ManejadorMisionPanel ManejadorMision = GameObject.FindObjectOfType<ManejadorMisionPanel>();
        if(ManejadorMision.GetComponent<Animator>().GetBool("Bajar") && ManejadorMision.NombreMisionMapa == this.name)
        {
            Transform PosTexto = ManejadorMision.transform.Find("Detalles_Mision").Find("Panel_Informacion_Y_Enemigos")
            .Find("Elementos_Mision").Find("Panel_Duracion_Descripcion_Enviar").Find("Texto_Expiracion");

            string TextoColocar = "Expira en : ";
            float TiempoP = Mathf.Round(ContadorTiempoEnMapaBatalla);
            if (TiempoP <= 59)
            {
                float Tiempo = Mathf.Round(ContadorTiempoEnMapaBatalla);
                TextoColocar += Tiempo + (Tiempo == 1 ? " segundo" : " segundos");
            }else if (TiempoP <= 3599)
            {
                float Tiempo = Mathf.Round(ContadorTiempoEnMapaBatalla / 60) ;
                TextoColocar += Tiempo + (Tiempo == 1 ? " minuto" : " minutos");
            }else if (TiempoP <= 86399)
            {
                float Tiempo = Mathf.Round(ContadorTiempoEnMapaBatalla / 3600) ;
                TextoColocar += Tiempo + (Tiempo == 1 ? " hora" : " horas");
            }
            else
            {
                float Tiempo = Mathf.Round(ContadorTiempoEnMapaBatalla / 86400) ;
                TextoColocar += Tiempo + (Tiempo == 1 ? " día" : " días");
            }

            PosTexto.GetComponent<Text>().text = TextoColocar;

        }

        if(ContadorTiempoEnMapaBatalla <= 0)
        {
            ManejadorMisionPanel MMP = GameObject.FindObjectOfType<ManejadorMisionPanel>();

            if (MMP.GetComponent<Animator>().GetBool("Bajar") && MMP.NombreMisionMapa == this.name)
            {
                MGM.ActivarDesactivarSeleccionablesDelMapa(true, true, true, true);
            }
            MGM.MisionesEnMapa.Remove(this);

            if (EsMisionUnica)
            {
                SpawnMisiones[] ManejadoresSpawn = GameObject.FindObjectsOfType<SpawnMisiones>();

                foreach (SpawnMisiones CSpawn in ManejadoresSpawn)
                {
                    CSpawn.MisionesBetadas.Remove(this);
                }
            }

            _ManejadorDeSpawn.GetComponent<SpawnMisiones>().GenerarMision = true;
            Destroy(Temporizador);
            Destroy(this.gameObject);
        }
        yield return new WaitForSeconds(.5f);
    }

    public void Ocultar(bool Mostrar = false)
    {
        this.GetComponent<SpriteRenderer>().enabled = Mostrar;
        this.transform.Find("Icono_Mision").GetComponent<SpriteRenderer>().enabled = Mostrar;
        this.transform.Find("Level").GetComponent<MeshRenderer>().enabled = Mostrar;
        this.transform.Find("Icono_Recompensa").GetComponent<SpriteRenderer>().enabled = Mostrar;
    }

    public override bool Equals(object other)
    {
        Mision Otro = (Mision)other;
        if (NombreBatalla == Otro.NombreBatalla)
        {
            return true;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}