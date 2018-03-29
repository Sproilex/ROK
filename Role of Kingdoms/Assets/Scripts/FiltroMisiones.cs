using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiltroMisiones : MonoBehaviour {

    private List<GameObject> _OpcionesCreadas = new List<GameObject>();
    [Tooltip("Nivel Maximo al que se puede llegar actualmente en el videojuego")]
    private Animator _animatorFiltro;
    ManejadorGeneralMundo MGM;

    /// <summary>
    /// Inicializacion de Variables y creacion de los filtros por level
    /// </summary>
	void Start(){
        _animatorFiltro = GetComponent<Animator>();

        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();

		int ResultadoCuadros = (int)Mathf.Round ((MGM.LevelMaximoActual) / 5);
        Transform PosAgrupador = transform.Find("Relleno").Find("Tags_Level").Find("Agrupador");
        GameObject Base = Instantiate(Resources.Load("Prefabs/Filtro/Opcion1") as GameObject, PosAgrupador) as GameObject;
        Base.name = "Base";
        for (int contadorPrefab = 0; contadorPrefab < ResultadoCuadros; contadorPrefab++) {

			if (GameObject.Find ("Opcion_F_L" + ((5 * (contadorPrefab + 1)) - 5) + "-" + (5 * (contadorPrefab + 1))) == null) {
                int Cantidad = contadorPrefab + 1;
				GameObject OpcionInstanciada = Instantiate (Resources.Load ("Prefabs/Filtro/Opcion1") as GameObject, PosAgrupador) as GameObject;
				OpcionInstanciada.GetComponent<Check_Filtro> ().NumeroComienzoMisiones = contadorPrefab;
				OpcionInstanciada.GetComponent<Check_Filtro> ().NumeroFinalMisiones = (5 * (contadorPrefab + 1));
                string ComplementoFinal = ((5 * (contadorPrefab + 1)) - 5) + "-" + (5 * (contadorPrefab + 1));
                OpcionInstanciada.transform.Find("Texto_Filtro_Level").GetComponent<Text>().text = "Level " + ComplementoFinal;
				OpcionInstanciada.name = "Opcion_F_L" + ComplementoFinal;
                _OpcionesCreadas.Add(OpcionInstanciada);

                VerticalLayoutGroup VLG = PosAgrupador.GetComponent<VerticalLayoutGroup>();

                if (Cantidad == 1)
                {
                    VLG.spacing = -500;
                }
                else if (Cantidad >= 6)
                {
                    VLG.spacing = -10;
                }
                else if (Cantidad == 2)
                {
                    VLG.spacing = -450;
                }
                else
                {
                    int Resultado = ((-11 * Cantidad) / Mathf.RoundToInt(Cantidad / 2)) * 10;
                    VLG.spacing = Resultado;
                }

			}
		}

	}

    /// <summary>
    /// Actualiza a las misiones del mapa con los filtros seleccionados (Solo level).
    /// </summary>
	public void ActualizarLevelsYAplicarFormato(){
        Check_Filtro[] ScriptsCF = GameObject.FindObjectsOfType(typeof(Check_Filtro)) as Check_Filtro[];
        foreach(Check_Filtro CS in ScriptsCF)
        {

            if (CS.gameObject.name.Contains("Opcion_F_L"))
            {
                CS.AlCambiarValor();
            }

        }
        if (GameObject.Find("Panel_Informacion_Misiones").GetComponent<Animator>().GetBool("Abajo"))
        {
            List<Mision> ObjetosMisiones = MGM.MisionesEnMapa;
            for (int contador = 0; contador < ObjetosMisiones.Count; contador++)
            {
                if (ObjetosMisiones[contador] != null)
                {
                    ObjetosMisiones[contador].GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }

    public void DesplegarOcultarPanel(bool cerrar = false)
    {
        if (!cerrar)
        {
            bool Izq = _animatorFiltro.GetBool("Izquierda");
            bool Der = _animatorFiltro.GetBool("Derecha");

            MGM.ActivarDesactivarSeleccionablesDelMapa(false, false, Izq, false);

            _animatorFiltro.SetBool("Izquierda", !Izq);
            _animatorFiltro.SetBool("Derecha", !Der);
        }
        else
        {
            _animatorFiltro.SetBool("Izquierda", false);
            _animatorFiltro.SetBool("Derecha", true);
        }
    }

}
