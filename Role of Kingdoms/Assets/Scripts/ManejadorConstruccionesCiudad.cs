using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorConstruccionesCiudad : MonoBehaviour {

    private Animator _animatorPanel;
    ManejadorGeneralMundo MGM;
	
    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        _animatorPanel = this.transform.GetComponent<Animator>();
    }

    public void DesplegarOcultarPanel(bool cerrar = false)
    {
        if (!cerrar)
        {
            if (!_animatorPanel.GetBool("Izquierda") && !_animatorPanel.GetBool("Derecha"))
            {
                _animatorPanel.SetBool("Derecha", true);
            }
            else
            {
                _animatorPanel.SetBool("Izquierda", !(_animatorPanel.GetBool("Izquierda")));
                _animatorPanel.SetBool("Derecha", !(_animatorPanel.GetBool("Derecha")));
            }
            Transform CuadriculaPos = GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones").transform.Find("Cuadricula");
            if (_animatorPanel.GetBool("Izquierda"))
            {

                int numeroHijos = CuadriculaPos.childCount;
                for (int n = 0; n < numeroHijos; n++)
                {
                    Destroy(CuadriculaPos.GetChild(n).gameObject);
                }

                MGM.ActivarDesactivarSeleccionablesDelMapa(false, false, true, false);

                GameObject BotonConstrucciones = GameObject.Find("Boton_Abrir_Construcciones_Ciudad");
                BotonConstrucciones.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                CuadriculaPos.parent.GetComponent<SistemaConstruccionesCiudad>().CrearCuadricula();
                MGM.ActivarDesactivarSeleccionablesDelMapa(false, false, false, false);

                GameObject BotonConstrucciones = GameObject.Find("Boton_Abrir_Construcciones_Ciudad");
                BotonConstrucciones.GetComponent<Button>().interactable = true;
                BotonConstrucciones.GetComponent<Image>().color = new Color(1,1,1,0);
            }
        }
        else
        {
            if (_animatorPanel.GetBool("Derecha"))
            {
                Transform CuadriculaPos = GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones").transform.Find("Cuadricula");
                int numeroHijos = CuadriculaPos.childCount;
                for (int n = 0; n < numeroHijos; n++)
                {
                    Destroy(CuadriculaPos.GetChild(n).gameObject);
                }
            }
            _animatorPanel.SetBool("Derecha",false);
            _animatorPanel.SetBool("Izquierda",true);

            GameObject BotonConstrucciones = GameObject.Find("Boton_Abrir_Construcciones_Ciudad");
            BotonConstrucciones.GetComponent<Button>().interactable = true;
            BotonConstrucciones.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
       
    }

}
