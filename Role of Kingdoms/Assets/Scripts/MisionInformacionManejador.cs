using UnityEngine;
using UnityEngine.UI;

public class MisionInformacionManejador : MonoBehaviour {

    Transform EsteTransform;
    private GameObject _BotonTerminarMision;
    private GameObject _TiempoRestanteMostarMision;
    private ManejadorGeneralMundo MGM;
    private GameObject _posMision;
    private float _TiempoRestanteMision;
    private float _TiempoTotalMision;
    private float _TiempoCombate;
    private float _TiempoTotalCombate;
    private bool _utilizarTiempoTotal = true;
    private int _porcentajeGanar;
    private int _ExperenciaADar;
    private int _CantidadMinimaEnergiaAQuitar;
    private int _CantidadMaximaEnergiaAQuitar;
    private int CantidadEnergiaAQuitarGenerada;
    private string _NombreMisionInformaciones;
    private Transform CiudadEnMapa;
    private bool Victoria;
    private Slider BarraCombate = null;
    private Vector3 TamanoOriginalBarra;
    //Informacion Soldados:
    public __informacionSoldados[] infoSoldados;
    private bool[] _EspacioTropas = new bool[3];
    private Vector3 Dir;
    private bool DireccionCambiada;
    private Vector3 pos1;
    private Vector3 pos2;
    private float _tiempoRestante;
    private Vector3 posOriginal;
    private LineRenderer _LineaSeguimiento;

    //Getters y Setters:

    public LineRenderer LineaSeguimiento
    {
        get { return _LineaSeguimiento; }
        set { _LineaSeguimiento = value; }
    }

    public bool UtilizarTiempoTotal
    {
        get { return _utilizarTiempoTotal; }
        set { _utilizarTiempoTotal = value; }
    }

    public float TiempoCombate
    {
        get { return _TiempoCombate; }
        set { _TiempoCombate = value; }
    }

    public float TiempoRestanteMovimiento
    {
        get { return _tiempoRestante; }
        set { _tiempoRestante = value; }
    }

    public GameObject BotonTerminarMision
    {
        get
        {
            return _BotonTerminarMision;
        }

        set
        {
            _BotonTerminarMision = value;
        }
    }

    public GameObject TiempoRestanteMostrarMision
    {
        get
        {
            return _TiempoRestanteMostarMision;
        }

        set
        {
            _TiempoRestanteMostarMision = value;
        }
    }

    public GameObject PosMision
    {
        get { return _posMision; }
        set { _posMision = value; }
    }

    public int porcentajeGanar
    {
        get
        {
            return _porcentajeGanar;
        }

        set
        {
            _porcentajeGanar = value;
        }
    }

    public int ExperenciaADar
    {
        get
        {
            return _ExperenciaADar;
        }

        set
        {
            _ExperenciaADar = value;
        }
    }

    public int CantidadMinimaEnergiaAQuitar
    {
        get
        {
            return _CantidadMinimaEnergiaAQuitar;
        }

        set
        {
            _CantidadMinimaEnergiaAQuitar = value;
        }
    }

    public int CantidadMaximaEnergiaAQuitar
    {
        get
        {
            return _CantidadMaximaEnergiaAQuitar;
        }

        set
        {
            _CantidadMaximaEnergiaAQuitar = value;
        }
    }

    public string NombreMisionInformaciones
    {
        get
        {
            return _NombreMisionInformaciones;
        }

        set
        {
            _NombreMisionInformaciones = value;
        }
    }

    public bool[] EspacioTropas
    {
        get
        {
            return _EspacioTropas;
        }

        set
        {
            _EspacioTropas = value;
        }
    }

    //--------------------------------------------

    /// <summary>
    /// Inicializacion de Variables, cambio de informacion de la tropa y calculos principales.
    /// </summary>
    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        EsteTransform = transform;
        Vector3 pos = new Vector3(_posMision.transform.localPosition.x, _posMision.transform.localPosition.y, transform.localPosition.z) - transform.localPosition;
        _TiempoTotalMision = (pos.magnitude * 100) / MGM.VelocidadMovimientoTropas;
        GameObject Temp = Instantiate(Resources.Load("Prefabs/Misiones/PrefabTemporizadorMisiones") as GameObject);
        TemporizadorTranscursoMisiones ScriptTemp = Temp.GetComponent<TemporizadorTranscursoMisiones>();
        ScriptTemp.TiempoTotal = _TiempoTotalMision;
        ScriptTemp.TiempoTotalCombate = _TiempoCombate;
        _TiempoTotalCombate = _TiempoCombate;
        _TiempoCombate = 0;
        ScriptTemp.MIMActual = this;
        ScriptTemp.BajarTiempo = true;
        ScriptTemp.LineaSeguimiento = LineaSeguimiento;
        CambiarDireccionDesplazamiento(_posMision.transform.localPosition);
        CiudadEnMapa = GameObject.FindGameObjectWithTag("Ciudad_En_Mapa").transform;
        this.GetComponent<SpriteRenderer>().sprite = MGM.IconoSoldadosMovimiento;
    }

    /// <summary>
    /// Mover la tropa
    /// </summary>
    void FixedUpdate()
    {
        EsteTransform.localPosition = Vector3.Lerp(posOriginal,Dir,_tiempoRestante);
       EsteTransform.position = new Vector3(EsteTransform.position.x, EsteTransform.position.y, -1f);
    }

    /// <summary>
    /// Cambiar direccion hacia donde se movera
    /// </summary>
    /// <param name="posObjetoObjetivo">Posicion hacia donde se movera</param>
    /// <param name="eliminar">Si se quiere eliminar el icono de la mision</param>
    void CambiarDireccionDesplazamiento(Vector3 posObjetoObjetivo, bool eliminar = false)
    {
        if(_TiempoCombate <= 0)
        {
            _tiempoRestante = 0;
            Dir = posObjetoObjetivo;
            posOriginal = this.transform.localPosition;
            if (eliminar)
            {
                _posMision.GetComponent<Mision>().Ocultar();
            }
        }
    }

    /// <summary>
    /// Cambia la direccion de desplazamiento cuando el tiempo llega a la mitad, activa el boton de terminar mision,
    ///  elimina a este objeto y modifica los datos mostrados en el panel de informacion de misiones cuando sea necesario.
    /// </summary>
    /// <returns></returns>
    public void VerificacionesMovimiento(float TiempoActualizado)
    {
        _TiempoRestanteMision = TiempoActualizado;
        float TiempoMostrar = _TiempoRestanteMision + (_utilizarTiempoTotal ? _TiempoTotalCombate : _TiempoCombate);
            string textoAgregar = "";
            if (TiempoMostrar >= 60 && TiempoMostrar <= 3599)
            {
                string HoraAMostrar = (Mathf.Round(TiempoMostrar / 60)).ToString();
                textoAgregar += HoraAMostrar +  (HoraAMostrar == "1" ? " Minuto" : " Minutos");
            }
            else if (TiempoMostrar >= 3600)
            {
                string HoraAMostrar = (Mathf.Round(TiempoMostrar / 3600)).ToString();
                textoAgregar += HoraAMostrar + (HoraAMostrar == "1" ? " Hora" : " Horas");
            }
            else if (TiempoMostrar <= 59)
            {
                textoAgregar += (Mathf.Round(TiempoMostrar)).ToString() + (TiempoMostrar == 1 ? " Segundo":" Segundos");
            }
            _TiempoRestanteMostarMision.GetComponent<Text>().text = textoAgregar;

        if (_TiempoRestanteMision <= (_TiempoTotalMision / 2) && !DireccionCambiada)
        {
            CambiarDireccionDesplazamiento(CiudadEnMapa.localPosition, true);
            DireccionCambiada = true;
        }
    }

    public void CalcularResultado()
    {
        int ResultadoBatalla = Random.Range(0, 100);
        Victoria = ResultadoBatalla <= _porcentajeGanar;
        Text ObjetoTextoResultado = _BotonTerminarMision.transform.parent.Find("Panel_Texto_Resultado").GetChild(0).GetComponent<Text>();
        ObjetoTextoResultado.text = Victoria ? MGM.MensajeVictoriaEnPanel : MGM.MensajeDerrotaEnPanel;
        ObjetoTextoResultado.color = Victoria ? MGM.ColorVictoria : MGM.ColorDerrota;

        Button BotonTerminar = _BotonTerminarMision.GetComponent<Button>();
        BotonTerminar.interactable = true;
        BotonTerminar.onClick.AddListener(delegate { Terminar(); });

        Destroy(this.GetComponent<SpriteRenderer>());
    }

    public void Terminar()
    {
        CantidadEnergiaAQuitarGenerada = System.Convert.ToInt32(Random.Range(_CantidadMinimaEnergiaAQuitar, _CantidadMaximaEnergiaAQuitar));
        for (int contador = 0; contador < EspacioTropas.Length; contador++)
        {
            if (EspacioTropas[contador])
            {
                Soldados ScriptAS1 = infoSoldados[contador].SoldadoEnLista.GetComponent<Soldados>();
                ScriptAS1.ActualizarResultadosDespuesBatalla(Victoria ? _ExperenciaADar : 0, CantidadEnergiaAQuitarGenerada);
                ScriptAS1.InfoActualSoldado.MisionCompletada = true;
                ScriptAS1.GetComponent<Button>().interactable = true;
                Soldados ScriptAS2 = infoSoldados[contador].SoldadoEnListaMision.GetComponent<Soldados>();
                ScriptAS2.ActualizarResultadosDespuesBatalla(Victoria ? _ExperenciaADar : 0, CantidadEnergiaAQuitarGenerada);
                ScriptAS2.InfoActualSoldado.MisionCompletada = true;
                ScriptAS2.GetComponent<Button>().interactable = true;
                ScriptAS2.PosibleEnviarAMision = true;
            }
        }
        Text TextoCantidadMisiones = GameObject.Find("Boton_Abrir_Panel_Informacion_Misiones").transform.Find("Panel_Cantidad_Misiones").GetChild(0).GetChild(0)
                .GetComponent<Text>();
        TextoCantidadMisiones.text = (System.Convert.ToInt32(TextoCantidadMisiones.text) - 1).ToString();
        DesplazamientoListadoInformacionMisiones[] SFlechas = GameObject.FindObjectsOfType<DesplazamientoListadoInformacionMisiones>();
        int contador2 = 1;
        int ContadorMisiones = 0;
        foreach (DesplazamientoListadoInformacionMisiones CS in SFlechas)
        {
            if(contador2 == 1)
            {
                CS.EliminarDeLista(GameObject.Find(_NombreMisionInformaciones));
            }
            CS.Misiones.Remove(GameObject.Find(_NombreMisionInformaciones));
            contador2++;
            ContadorMisiones = CS.Misiones.Count;
        }

        if (ContadorMisiones == 0)
        {
            GameObject.Find("Panel_Informacion_Misiones").transform.Find("Encabezado").GetChild(0).GetChild(0).GetComponent<Text>().text = "Sin Misiones";
        }

        Mision InfMision = _posMision.GetComponent<Mision>();
        SpawnMisiones[] ManejadoresSpawn = GameObject.FindObjectsOfType<SpawnMisiones>();

        foreach(SpawnMisiones CSpawn in ManejadoresSpawn)
        {
            CSpawn.MisionesBetadas.Remove(InfMision);
        }

        MGM.MisionesEnMapa.Remove(InfMision);
        InfMision.ManejadorDeSpawn.GetComponent<SpawnMisiones>().GenerarMision = true;
        Destroy(_posMision);
        Destroy(GameObject.Find(_NombreMisionInformaciones));
        Destroy(this.gameObject);
    }

    public void ManejarBarra()
    {
        if (BarraCombate == null)
        {
            Vector3 posBarra = Camera.main.WorldToScreenPoint(transform.GetChild(0).position);
            BarraCombate = Instantiate(Resources.Load(@"Prefabs\Construcciones_Ciudad\Progreso_Tiempo_Prefab") as GameObject, transform.GetChild(0))
                .GetComponent<Slider>();
            BarraCombate.transform.SetParent(GameObject.Find("Interfaz").transform.Find("Barras_De_Carga"));
            BarraCombate.transform.position = posBarra;
            TamanoOriginalBarra = BarraCombate.transform.localScale;
        }

        BarraCombate.gameObject.SetActive(GameObject.FindGameObjectWithTag("Mapa"));

        float CValor = Camera.main.orthographicSize;
        BarraCombate.transform.localScale = new Vector3((TamanoOriginalBarra.x * 100) / CValor, (TamanoOriginalBarra.y * 100) / CValor, 0) / 15;

        BarraCombate.transform.position = Camera.main.WorldToScreenPoint(transform.GetChild(0).position);
        BarraCombate.value = 100 - (int)Mathf.Abs((_TiempoCombate * 100) / _TiempoTotalCombate);

        if(BarraCombate != null && BarraCombate.value >= 100)
        {
            Destroy(BarraCombate.gameObject);
        }

    }

}
