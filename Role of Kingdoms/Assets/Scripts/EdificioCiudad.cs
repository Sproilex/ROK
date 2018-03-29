using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EdificioCiudad : MonoBehaviour {

    private bool _Activo;
    private FuncionesEdificios EdificioInterfaz;
    ManejadorGeneralMundo MGM;
    GraphicRaycaster _GR;

    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        _GR = MGM.GraphicRaycasterPrincipal;
        InicializarVariablesSegunTipoEdificio();
    }

    public bool Activo
    {
        get { return _Activo; }
        set { _Activo = value; VerificarSiActivar(); }
    }

    public void InicializarVariablesSegunTipoEdificio()
    {
        EdificioInterfaz = GetComponent<CuartelGeneralConstruccionScript>();
        Transform posBoton = GameObject.Find("Contenedor_Informacion_Creacion_Soldados").transform.Find("Borde_Boton_Crear_Soldado").Find("Boton_Crear_Soldado");
        posBoton.GetComponent<Button>().onClick.AddListener(delegate { EjecutarBotonAccion1(); });
    }

    void OnMouseUp()
    {
        List<RaycastResult> _resultadoRaycast = new List<RaycastResult>();
        PointerEventData _punteroInfo = new PointerEventData(null);
        _punteroInfo.position = Input.mousePosition;
        _GR.Raycast(_punteroInfo, _resultadoRaycast);

        if(_resultadoRaycast.Count == 0 && Activo)
        {
            EdificioInterfaz.AbrirPanelDelEdificio();
        }   
    }

    void EjecutarBotonAccion1()
    {
        EdificioInterfaz.FuncionBotonAccion1();
    }

    void VerificarSiActivar()
    {
        bool Activar = GameObject.FindObjectOfType<ManejadorConstruccionesCiudad>().GetComponent<Animator>().GetBool("Izquierda");
        gameObject.GetComponent<BoxCollider2D>().enabled = Activar;
    }

}
