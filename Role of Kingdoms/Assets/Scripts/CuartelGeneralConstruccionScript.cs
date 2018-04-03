using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CuartelGeneralConstruccionScript : FuncionesEdificios
{

    private float _Contador;
    private bool BajarTiempo;
    Transform _posPanelColaConstruccion;
    Transform posPanelCompleto;
    ManejadorGeneralMundo MGM;
    bool _tiempoEnCero;

    //Getters y Setters:

    public bool tiempoEnCero
    {
        get { return _tiempoEnCero; }
        set { _tiempoEnCero = value; }
    }

    public float Contador
    {
        get { return _Contador; }
        set { _Contador = value; }
    }

    //Metodos

    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        posPanelCompleto = GameObject.FindGameObjectWithTag("Interfaz_Panel_Soldados").transform;
        _Contador = ManejadorGeneralMundo.CambiarTiempoAFormato(MGM.FormatoTiempoGeneracionSoldados, MGM.TiempoGeneracionSoldados);
        _posPanelColaConstruccion = GameObject.FindGameObjectWithTag("Interfaz_Panel_Soldados").transform.Find("Contenedor_Informacion_Creacion_Soldados")
        .Find("Panel_Estado_Cola_Construccion");
    }

    public override void AbrirPanelDelEdificio()
    {
        posPanelCompleto.GetComponent<ManejadorPanelInformacionCuartelSoldados>().DesplegarOcultarPanel();
        string complementoFinal = "";

        if(MGM.FormatoTiempoGeneracionSoldados == EFormatoTiempo.Dias)
        {
            complementoFinal = (MGM.TiempoGeneracionSoldados == 1 ? " día" : " días");
        }else if (MGM.FormatoTiempoGeneracionSoldados == EFormatoTiempo.Horas)
        {
            complementoFinal = (MGM.TiempoGeneracionSoldados == 1 ? " hora" : " horas");
        }else if (MGM.FormatoTiempoGeneracionSoldados == EFormatoTiempo.Minutos)
        {
            complementoFinal = (MGM.TiempoGeneracionSoldados == 1 ? " minuto" : " minutos");
        }
        else
        {
            complementoFinal = (MGM.TiempoGeneracionSoldados == 1 ? " segundo" : " segundos");
        }

        posPanelCompleto.Find("Contenedor_Informacion_Creacion_Soldados").Find("Panel_Tiempo_Creacion").Find("Texto_Tiempo_Creacion")
        .GetComponent<Text>().text = MGM.TiempoGeneracionSoldados + complementoFinal;
    }

    public override void FuncionBotonAccion1()
    {
        _posPanelColaConstruccion.parent.Find("Borde_Boton_Crear_Soldado").GetChild(0).GetComponent<Button>().interactable = false;
        BajarTiempo = true;
        StartCoroutine(ComprobarParaCreacionCartaSoldado());
        GameObject Temp = Instantiate(Resources.Load("Prefabs/Soldados/Temporizador_Creacion_Soldado") as GameObject);
        Temp.GetComponent<TemporizadorCreacionSoldado>().TiempoTotal = Contador;
        Temp.GetComponent<TemporizadorCreacionSoldado>().Construccion = this.gameObject;
    }

    IEnumerator ComprobarParaCreacionCartaSoldado()
    {
        int contadorEnMinutosOSegundos = _Contador <= 59 ? Convert.ToInt32(_Contador) : Convert.ToInt32(Mathf.Round(_Contador / 60));
        string tiempoEscrito = _Contador <= 59 ? contadorEnMinutosOSegundos == 1 ? " segundo" : " segundos" :
                                          contadorEnMinutosOSegundos == 1 ? " minuto" : " minutos";
        if (_tiempoEnCero)
        {
            BajarTiempo = false;
            _posPanelColaConstruccion.Find("Texto_Estado_Cola_Construccion").GetComponent<Text>().text = "Nada en Cola";
            _posPanelColaConstruccion.Find("SubPanel_Estado_Cola_Construccion").Find("Texto_Tiempo_Cola_Construccion")
            .GetComponent<Text>().text = "";
            _posPanelColaConstruccion.parent.Find("Panel_Cartas_Disponibles").Find("SubPanel_Cartas_Disponibles").Find("Texto_Cartas_Disponibles")
            .GetComponent<Text>().text = MGM.NumeroSoldadosActuales + "/" + (MGM.NumeroMaximoSoldados);
            _posPanelColaConstruccion.parent.Find("Panel_Cartas_Por_Crear").Find("SubPanel_Cartas_Por_Crear").Find("Texto_Cartas_Por_Crear")
            .GetComponent<Text>().text = (MGM.NumeroMaximoSoldados - MGM.NumeroSoldadosActuales) + "/" + (MGM.NumeroMaximoSoldados);
            _posPanelColaConstruccion.parent.Find("Borde_Boton_Crear_Soldado").GetChild(0).GetComponent<Button>().interactable = MGM.NumeroSoldadosActuales < MGM.NumeroMaximoSoldados;
            _Contador = ManejadorGeneralMundo.CambiarTiempoAFormato(MGM.FormatoTiempoGeneracionSoldados, MGM.TiempoGeneracionSoldados);
            _tiempoEnCero = false;
        }
        else
        {
            _posPanelColaConstruccion.Find("Texto_Estado_Cola_Construccion").GetComponent<Text>().text = "En Proceso";
            _posPanelColaConstruccion.Find("SubPanel_Estado_Cola_Construccion").Find("Texto_Tiempo_Cola_Construccion")
            .GetComponent<Text>().text = contadorEnMinutosOSegundos + tiempoEscrito;
        }

        yield return new WaitForSeconds(.5f);
        if (BajarTiempo)
            StartCoroutine(ComprobarParaCreacionCartaSoldado());
    }

    void OnEnable()
    {
        if (BajarTiempo)
            StartCoroutine(ComprobarParaCreacionCartaSoldado());
    }

}
