using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Soldados : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public __informacionSoldados InfoActualSoldado;

    private float _contadorIncrementoEnergia;
    private float reponerContadorIncrementoEnergia;
    private ManejadorGeneralMundo MGM;
    private Slider BarraEnergia;
    private bool _PosibleEnviarAMision;
    private bool _NoEsNivelMision;
    private AudioSource Reproductor;
    private Sprite BarraEnergiaImagenNormal;
    private Sprite BarraEnergiaImagenLesion;
    private Image ImagenBarraEnergia;
    //Drag and Drop:
    private Vector3 posicionInicial;
    private Transform _PadreOriginal;
    private GraphicRaycaster _GR;
    private PointerEventData _punteroInfo;
    private List<RaycastResult> _resultadoRaycast;

    //Getters y Setters:

    public bool NoEsNivelMision
    {
        get { return _NoEsNivelMision; }
        set { _NoEsNivelMision = value; }
    }

    public bool PosibleEnviarAMision
    {
        get { return _PosibleEnviarAMision; }
        set { _PosibleEnviarAMision = value; }
    }

    public Transform PadreOriginal
    {
        get { return _PadreOriginal; }
        set { _PadreOriginal = value; }
    }

    /// <summary>
    /// Inicializacion de Variables
    /// </summary>
    void Start()
    {
        MGM = GameObject.Find("ManejadorGeneralMundo").GetComponent<ManejadorGeneralMundo>();
        foreach(Sprite CImagen in Resources.LoadAll<Sprite>(@"Iconos\Soldados\atlas_cartas_soldados"))
        {
            if(CImagen.name == "Barra_Energia_Baja")
            {
                BarraEnergiaImagenLesion = CImagen;
            }else if(CImagen.name == "Barra_Energia_Completa")
            {
                BarraEnergiaImagenNormal = CImagen;
            }
        }
        BarraEnergia = transform.Find("Barra_Energia_Soldado").GetComponent<Slider>();
        ImagenBarraEnergia = BarraEnergia.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        _contadorIncrementoEnergia = MGM.VelocidadRecargaEnergiaSoldados;
        reponerContadorIncrementoEnergia = _contadorIncrementoEnergia;
        _resultadoRaycast = new List<RaycastResult>();
        _punteroInfo = new PointerEventData(null);
        _GR = MGM.GraphicRaycasterPrincipal;
        Reproductor = GameObject.Find("Reproductor_Voces").GetComponent<AudioSource>();
        StartCoroutine(ModificarDatosSoldados());
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (_PosibleEnviarAMision && !InfoActualSoldado.EnMision && !NoEsNivelMision)
        {
            posicionInicial = this.transform.position;
            _PadreOriginal = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent.parent.parent, false);
            this.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        }
        else
        {
            AlPulsarEnSoldado();
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (_PosibleEnviarAMision && !InfoActualSoldado.EnMision && !_NoEsNivelMision)
        {
            this.transform.position = Input.mousePosition;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (_PosibleEnviarAMision && !_NoEsNivelMision && !InfoActualSoldado.EnMision)
        {
            if (Input.GetMouseButtonUp(0) && !InfoActualSoldado.EnMision)
            {
                _punteroInfo.position = Input.mousePosition;
                _GR.Raycast(_punteroInfo, _resultadoRaycast);
                if (_resultadoRaycast.Count > 0)
                {
                    GameObject ObjetoInfoDetalles = _resultadoRaycast.Find(x => x.gameObject.name == "Panel_Mostrar_Detalles").gameObject;
                    GameObject ObjetoElementos = _resultadoRaycast.Find(x => x.gameObject.name == "Elementos_Mision").gameObject;
                    if (ObjetoInfoDetalles != null)
                    {
                        ObjetoInfoDetalles.GetComponent<ManejadorDetallesPanelMision>().CambiarDatos(InfoActualSoldado);
                    }
                    else if (ObjetoElementos != null)
                    {
                        AgregarTropaAMision();
                    }
                }
            }
            this.GetComponent<RectTransform>().pivot = new Vector2();
            this.transform.SetParent(_PadreOriginal, false);
            this.transform.position = posicionInicial;
            _resultadoRaycast = new List<RaycastResult>();
        }
            
    }

    /// <summary>
    /// Aumenta la Energia del Soldado y cambia su valor fisicamente
    /// </summary>
    /// <returns></returns>
    IEnumerator ModificarDatosSoldados()
    {
        _contadorIncrementoEnergia -= 1;

        if (_contadorIncrementoEnergia <= 0)
        {
            if (InfoActualSoldado.Energia < 100 && !InfoActualSoldado.Lesion)
            {
                InfoActualSoldado.Energia++;
                _contadorIncrementoEnergia = reponerContadorIncrementoEnergia;
            }
            else if (InfoActualSoldado.Energia < 100 && InfoActualSoldado.Lesion)
            {
                InfoActualSoldado.Energia++;
                _contadorIncrementoEnergia = reponerContadorIncrementoEnergia * 2;
            }

        }

        if (InfoActualSoldado.Energia < 0)
        {
            InfoActualSoldado.Energia = 0;
        }

        if (InfoActualSoldado.Energia >= 26)
        {
            InfoActualSoldado.Lesion = false;
            ImagenBarraEnergia.sprite = BarraEnergiaImagenNormal;
        }
        else
        {
            InfoActualSoldado.Lesion = true;
            ImagenBarraEnergia.sprite = BarraEnergiaImagenLesion;
        }

        if(InfoActualSoldado.Energia <= 0)
        {
            DesplazamientoListadoTropasMision[] STropas = GameObject.FindObjectsOfType<DesplazamientoListadoTropasMision>();
            foreach(DesplazamientoListadoTropasMision CStropas in STropas)
            {
                if (_PosibleEnviarAMision && CStropas.transform.parent.name == "Detalles_Mision")
                {
                    CStropas.EliminarSoldadoMision(this.gameObject);
                    break;
                }else if(!_PosibleEnviarAMision && CStropas.transform.parent.name == "Panel_Contenedor_Soldados")
                {
                    CStropas.EliminarSoldadoLista(this.gameObject);
                    MGM.NumeroSoldadosActuales--;
                    break;
                }
            }
            Destroy(this.gameObject);
        }
        BarraEnergia.value = InfoActualSoldado.Energia;

        Soldados OtroSoldado = _PosibleEnviarAMision ? InfoActualSoldado.SoldadoEnLista.GetComponent<Soldados>():
                                                       InfoActualSoldado.SoldadoEnListaMision.GetComponent<Soldados>();
        int EnergiaOtroSoldado = OtroSoldado.InfoActualSoldado.Energia;

        if (EnergiaOtroSoldado > InfoActualSoldado.Energia)
        {
            InfoActualSoldado.Energia = EnergiaOtroSoldado;
        }
        else if(EnergiaOtroSoldado < InfoActualSoldado.Energia)
        {
            OtroSoldado.InfoActualSoldado.Energia = InfoActualSoldado.Energia;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(ModificarDatosSoldados());
    }

    /// <summary>
    /// Agrega la informacion de la tropa a la Mision Actual Mostrada (Si existe)
    /// </summary>
    public void AgregarTropaAMision()
    {
        Mision SMision = GameObject.Find(MGM.MisionPanel.NombreMisionMapa).
                        GetComponent<Mision>();
        bool[] Tropas = SMision.EspacioTropas;
        int contador = 0;
        

        foreach (bool CSlotTropa in Tropas){
            if (!CSlotTropa && !InfoActualSoldado.EnMision)
            {
                Tropas[contador] = true;
                QuitarSoldado[] IconosSoldados = GameObject.FindObjectsOfType<QuitarSoldado>();
                foreach (QuitarSoldado CQS in IconosSoldados)
                {
                    if (CQS.gameObject.name == "Espacio_Soldado_" + (contador + 1))
                    {
                        Image IconoSoldado = CQS.GetComponent<Image>();
                        IconoSoldado.color = new Color(0.074f, 1, 0.348f, 1);
                        IconoSoldado.sprite = Resources.Load<Sprite>("Iconos/Interfaz/CheckIcon");
                    }
                }

                //Poner Icono de En Mision encima del soldado

                Soldados[] todosSoldados = GameObject.FindObjectsOfType<Soldados>();
                foreach (Soldados CSoldado in todosSoldados)
                {
                    if (CSoldado.InfoActualSoldado == InfoActualSoldado && CSoldado.gameObject != this.gameObject)
                    {
                        //Poner Icono de En Mision encima del soldado
                        CSoldado.GetComponent<Button>().interactable = false;
                    }
                }

                //Guardar Información de la Tropa
                SMision.infoSoldadosDisponibles[contador] = InfoActualSoldado;
                InfoActualSoldado.EnMision = true;
                this.GetComponent<Button>().interactable = false;
                SMision.ActualizarResultados();
                break;

            }
                contador++;
            }
    }

    /// <summary>
    /// Vuelve a activar al soldado permitiendo volver a seleccionarlo
    /// </summary>
    /// <param name="Desactivar">Si se quiere desactivar al soldado</param>
    public void ActivarTropa(bool Desactivar = false)
    {
        InfoActualSoldado.EnMision = Desactivar;
        GetComponent<Button>().interactable = !Desactivar;
    }

    /// <summary>
    /// Cambia las estadisticas del soldado
    /// </summary>
    /// <param name="EXPR">Cantidad de Experiencia a añadir</param>
    /// <param name="EnergiaR">Cantidad de Energia a Restar</param>
    public void ActualizarResultadosDespuesBatalla(int EXPR, int EnergiaR)
    {
        InfoActualSoldado.ActualizarDatos(EnergiaR,EXPR, false);
        ActualizarInformacionFisicamente();
    }

    /// <summary>
    /// Modifica la Informacion del Soldado en la carta
    /// </summary>
    public void ActualizarInformacionFisicamente()
    {
        transform.Find("Icono_Soldado").GetComponent<Image>().sprite = InfoActualSoldado.Icono;
        Transform Detalles = transform.Find("Detalles_Diseño");
        Detalles.Find("Level_Soldado").GetComponent<Text>().text = InfoActualSoldado.Nivel.ToString();
        Detalles.Find("Clase_Soldado").GetComponent<Text>().text = InfoActualSoldado.Clase;
        Detalles.Find("Nombre_Soldado").GetComponent<Text>().text = InfoActualSoldado.Nombre;
        _PadreOriginal = _PosibleEnviarAMision ? this.transform.parent : null;
    }

    /// <summary>
    /// Al hacer clic en el soldado verifica si debe mostrar la info o agregar la tropa para la mision
    /// </summary>
    public void AlPulsarEnSoldado()
    {
        if (!_PosibleEnviarAMision && !NoEsNivelMision)
        {
            MostrarDatosSoldados();
        }
        else if(_PosibleEnviarAMision && NoEsNivelMision && !InfoActualSoldado.EnMision && !Reproductor.isPlaying)
        {
            AudioClip Sonido = MGM.VocesAdvertenciaNivelNoAdmitido
                [Random.Range(0, MGM.VocesAdvertenciaNivelNoAdmitido.Count - 1)];
            Reproductor.PlayOneShot(Sonido);
        }
    }

    /// <summary>
    /// Muestra la informacion del soldado en el panel comun de los mismos
    /// </summary>
    private void MostrarDatosSoldados()
    {
        Transform PanelInformacionPrincipal = GameObject.FindGameObjectWithTag("Agrupador_Informacion_Soldados").transform;
        string[] NombreInformacionReemplazar = new string[]{ "Raza", "Beneficio", "Level", "Heroe", "Vida", "Daño", "Energia", "Experiencia", "Poderes", "Puntos" };
        string[] InformacionReemplazar = new string[] {InfoActualSoldado.Raza,InfoActualSoldado.Clase,InfoActualSoldado.Nivel.ToString(),
                                                        InfoActualSoldado.Heroe ? "Si":"No", InfoActualSoldado.Vida.ToString(),InfoActualSoldado.Daño.ToString(),
                                                        InfoActualSoldado.Energia.ToString(),InfoActualSoldado.EXP.ToString() + "/" + InfoActualSoldado.EXPAL.ToString(),
                                                        InfoActualSoldado.Poderes.ToString(),"0"};
        for(int n = 0; n < NombreInformacionReemplazar.Length; n++)
        {
            PanelInformacionPrincipal.Find("Mostrador_" + NombreInformacionReemplazar[n]).Find("Soldado_Actual_" + NombreInformacionReemplazar[n]).Find("Texto_Soldado_Actual_" + 
                                            NombreInformacionReemplazar[n]).GetComponent<Text>().text = InformacionReemplazar[n];
        }
    }

    /// <summary>
    /// Cambia el color del borde para indicar si esta o no seleccionada la carta
    /// </summary>
    /// <param name="Verificar">Si se quiere verificar o si se quiere hacer manualmente</param>
    /// <param name="activar">Si se quiere activar o desactivar directamente</param>
    //public void CambiarColorSeleccion(bool Verificar, bool activar = false)
    //{
    //    Color orig = _PadreOriginal.GetComponent<Image>().color;
    //    float r = orig.r;
    //    float g = orig.g;
    //    float b = orig.b;
    //    Soldados[] Solds = GameObject.FindObjectsOfType<Soldados>();

    //    foreach(Soldados CSoldado in Solds)
    //    {
    //        if (CSoldado._PosibleEnviarAMision)
    //        {
    //            CSoldado._PadreOriginal.GetComponent<Image>().color = new Color(r, g, b, 0);
    //        }
    //    }

    //    if (Verificar)
    //    {
    //        float z = orig.a == 1 ? 0 : 1;
    //        _PadreOriginal.GetComponent<Image>().color = new Color(r, g, b, z);
    //    }
    //    else
    //    {
    //        float z = !activar ? 0 : 1;
    //        _PadreOriginal.GetComponent<Image>().color = new Color(r, g, b, z);
    //    }
    //}

}
