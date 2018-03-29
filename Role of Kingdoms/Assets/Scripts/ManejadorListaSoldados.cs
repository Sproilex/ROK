using UnityEngine;
using UnityEngine.UI;

public class ManejadorListaSoldados : MonoBehaviour {

    //Informacion de este objeto:
    private Animator _animatorMLS;
    private ManejadorGeneralMundo MGM;

    /// <summary>
    /// Inicializacion de Variables
    /// </summary>
    void Start()
    {
        _animatorMLS = gameObject.GetComponent<Animator>();
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
    }

    public void DesplegarOcultarPanel(bool Cerrar = false)
    {
        if (!Cerrar)
        {
            if (_animatorMLS.GetBool("Subir") == false && _animatorMLS.GetBool("Bajar") == false)
            {
                MGM.ActivarDesactivarSeleccionablesDelMapa(false,false, false,true);
                _animatorMLS.SetBool("Subir", true);
                _animatorMLS.SetBool("Bajar", false);
            }
           else
            {
                bool Bajar = !_animatorMLS.GetBool("Bajar");
                MGM.ActivarDesactivarSeleccionablesDelMapa(false, false, Bajar, false);
                MGM.ActivarDesactivarMovimientoMapa(GameObject.FindGameObjectWithTag("Mapa").name, Bajar);
                _animatorMLS.SetBool("Subir", !Bajar);
                _animatorMLS.SetBool("Bajar", Bajar);
            }

            GameObject BotonSoldados = GameObject.Find("Boton_Desplegar_Panel_Soldados");
            BotonSoldados.GetComponent<Button>().interactable = _animatorMLS.GetBool("Bajar");
            BotonSoldados.GetComponent<Image>().color = new Color(1, 1, 1, _animatorMLS.GetBool("Subir") ? 0 : 1);
        }
        else
        {
            _animatorMLS.SetBool("Subir", false);
            _animatorMLS.SetBool("Bajar", true);

            GameObject BotonSoldados = GameObject.Find("Boton_Desplegar_Panel_Soldados");
            BotonSoldados.GetComponent<Button>().interactable = true;
            BotonSoldados.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

}
