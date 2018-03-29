using UnityEngine;
using UnityEngine.UI;

public class EnviarAMision : MonoBehaviour {

    private Mision ScriptMM;
    private GameObject _PrefabMisionesInformacion;
    private GameObject _PrefabManejador;
    private ManejadorGeneralMundo MGM;

    /// <summary>
    /// Inicializacion de Variables
    /// </summary>
    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        _PrefabMisionesInformacion = Resources.Load("Prefabs/Misiones/InformacionMisionPrefab") as GameObject;
        _PrefabManejador = Resources.Load("Prefabs/Misiones/MisionInformacionPrefabManejador") as GameObject;
    }

    /// <summary>
    /// Guardar informacion de soldados, crear informacion de la mision, bloquear la mision y limpiar la mision mostrada.
    /// </summary>
    public void Enviar()
    {
        //Verificar si hay almenos un soldado:
        string NombreMision = MGM.MisionPanel.NombreMisionMapa;
        ScriptMM = GameObject.Find(NombreMision).GetComponent<Mision>();
        bool HaySoldados = false;
        for (int contador = 0; contador < ScriptMM.EspacioTropas.Length; contador++)
        {
            if (ScriptMM.EspacioTropas[contador])
            {
                HaySoldados = true;
                break;
            }
        }

        if (HaySoldados)
        {
            ManejadorGeneralMundo ManejadorG = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();

            //Bloquear Soldados:
            for (int n = 0; n < ScriptMM.EspacioTropas.Length; n++)
            {
                if (ScriptMM.infoSoldadosDisponibles[n].Nombre != null)
                {
                    ScriptMM.infoSoldadosDisponibles[n].SoldadoEnLista.GetComponent<Soldados>().InfoActualSoldado.MisionCompletada = false;
                    ScriptMM.infoSoldadosDisponibles[n].SoldadoEnListaMision.GetComponent<Soldados>().InfoActualSoldado.MisionCompletada = false;
                }
            }
            //Instanciar el prefab en el panel de informacion de misiones:
            Transform PosAgrupador = GameObject.FindGameObjectWithTag("Agrupador_Panel_Informacion_Misiones").transform;
            GameObject ObjetoDeLaMision = null;
            for (int contador = 0; contador < PosAgrupador.childCount; contador++)
            {
                if(PosAgrupador.GetChild(contador).childCount == 0 && PosAgrupador.GetChild(contador).name != "Espacio_Misiones_Ocultas")
                {
                    ObjetoDeLaMision = Instantiate(_PrefabMisionesInformacion, PosAgrupador.GetChild(contador));
                    break;
                }else if(contador + 1 == PosAgrupador.childCount)
                {
                    ObjetoDeLaMision = Instantiate(_PrefabMisionesInformacion, PosAgrupador.GetChild(contador));
                }
            }

            //Cambiar Texto de Informacion:

            PosAgrupador.parent.parent.Find("Encabezado").GetChild(0).GetChild(0).GetComponent<Text>().text = "Resumen de Misiones Enviadas";
            //Cambiar Datos:
            Transform PosInformacionP = ObjetoDeLaMision.transform.Find("SubPanel_Informacion");
            PosInformacionP.Find("Texto_Mision_Espacio").GetComponent<Text>().text = ScriptMM.NombreBatalla;
            PosInformacionP.Find("Icono_Mision").GetComponent<Image>().sprite = ScriptMM.IconoMision;
            string tiempoTotalMisionEnTexto = ScriptMM.TiempoFinal <= 60 ? (Mathf.Round(ScriptMM.TiempoFinal)).ToString() + " Segundos" : 
                                                                           (Mathf.Round(ScriptMM.TiempoFinal / 60)) > 60 ?
                                                                                (Mathf.Round((ScriptMM.TiempoFinal / 60) / 60)).ToString() + " Horas" :
                                                                                (Mathf.Round(ScriptMM.TiempoFinal / 60)).ToString() + " Minutos";

            PosInformacionP.Find("Texto_Restante_Mision_Espacio").GetComponent<Text>().text = tiempoTotalMisionEnTexto;
            PosInformacionP.Find("Panel_Recompensas").Find("Texto_Recompensa_Experiencia_Espacio")
                .GetComponent<Text>().text = ScriptMM.EXPADarASoldados.ToString();

            for(int contador = 0; contador < 3; contador++)
            {
                Transform PosCarta = PosInformacionP.Find("Espacio_Soldado_" + (contador + 1)).Find("Carta");
                if (contador < ScriptMM.EspacioTropas.Length && ScriptMM.EspacioTropas[contador])
                {
                    PosCarta.Find("Icono_Soldado").GetComponent<Image>().sprite = ScriptMM.infoSoldadosDisponibles[contador].Icono;
                    Transform PosDetalles = PosCarta.Find("Detalles_Diseño");
                    PosDetalles.Find("Level_Soldado").GetComponent<Text>().text = ScriptMM.infoSoldadosDisponibles[contador].Nivel.ToString();
                    PosDetalles.Find("Nombre_Soldado").GetComponent<Text>().text = ScriptMM.infoSoldadosDisponibles[contador].Nombre.ToString();
                }
                else
                {
                    Destroy(PosCarta.gameObject);
                }
            }

            //Instanciar y agregar informacion al manejador de la mision:
            ManejadorPanelInformacionMisiones MPIM = GameObject.FindObjectOfType<ManejadorPanelInformacionMisiones>();
            string ComplementoFinal = MPIM.ContadorActualMisiones.ToString();
            MPIM.ContadorActualMisiones++;
            DesplazamientoListadoInformacionMisiones[] SFlechas = GameObject.FindObjectsOfType<DesplazamientoListadoInformacionMisiones>();
            foreach (DesplazamientoListadoInformacionMisiones CS in SFlechas)
            {
                CS.Misiones.Add(ObjetoDeLaMision);
                if(CS.gameObject.name == "Boton_Bajar" && CS.Misiones.Count > 3)
                {
                    CS.GetComponent<Button>().interactable = true;
                }
            }

            Vector3 PosCiudad = GameObject.FindGameObjectWithTag("Ciudad_En_Mapa").transform.position;
            GameObject ObjetoManejadorMision = Instantiate(_PrefabManejador,new Vector3(PosCiudad.x,PosCiudad.y,-4f),
                                                            new Quaternion(),ScriptMM.transform.parent);
            MisionInformacionManejador MIM = ObjetoManejadorMision.GetComponent<MisionInformacionManejador>();
            MIM.BotonTerminarMision = PosInformacionP.parent.Find("SubPanel_Resultado").Find("Boton_Finalizar").gameObject;
            MIM.PosMision = ScriptMM.gameObject;
            MIM.porcentajeGanar = ScriptMM.ProbabilidadExitoJugador;
            MIM.TiempoRestanteMostrarMision = PosInformacionP.Find("Texto_Restante_Mision_Espacio").gameObject;
            MIM.infoSoldados = ScriptMM.infoSoldadosDisponibles;
            MIM.EspacioTropas = ScriptMM.EspacioTropas;
            MIM.ExperenciaADar = ScriptMM.EXPADarASoldados;
            MIM.CantidadMinimaEnergiaAQuitar = ScriptMM.CantidadMinimaEnergiaRestarSoldados;
            MIM.CantidadMaximaEnergiaAQuitar = ScriptMM.CantidadMaximaEnergiaRestarSoldados;
            MIM.TiempoCombate = ManejadorGeneralMundo.CambiarTiempoAFormato(ScriptMM.FormatoTiempoPeleaBatalla,ScriptMM.TiempoPeleaBatalla);
            MIM.NombreMisionInformaciones = ComplementoFinal;

            GameObject Linea = Instantiate(Resources.Load(@"Prefabs\Misiones\LineaSeguimientoMision") as GameObject);
            Linea.transform.SetParent(ObjetoManejadorMision.transform);
            LineRenderer LR = Linea.GetComponent<LineRenderer>();
            LR.SetPosition(0,new Vector3(PosCiudad.x, PosCiudad.y,-0.01f));
            LR.SetPosition(1, new Vector3(ScriptMM.transform.position.x, ScriptMM.transform.position.y,-0.01f));
            MIM.LineaSeguimiento = LR;

            //Cambiar nombre a cada objeto a uno unico:
            ObjetoDeLaMision.name = ComplementoFinal;
            ObjetoManejadorMision.name = "ObjetoManejadorMision:" + ComplementoFinal;

            GameObject _posMision = ScriptMM.gameObject;
            _posMision.GetComponent<BoxCollider2D>().enabled = false;
            _posMision.GetComponent<Mision>().SoldadoEnCamino = true;
            _posMision.transform.Find("Icono_Mision").GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f);
            ManejadorG.ActivarDesactivarSeleccionablesDelMapa(true, false, true, true);
            ManejadorG.CerrarYLimpiarMisionMostrada(false);
            Text TextoCantidadMisiones = GameObject.FindGameObjectWithTag("TextoCantidadMisionesEnviadas").GetComponent<Text>();
            TextoCantidadMisiones.text = (System.Convert.ToInt32(TextoCantidadMisiones.text) + 1).ToString();
        }

    }

}
