using UnityEngine;
using UnityEngine.UI;

public class QuitarSoldado : MonoBehaviour {

    [Tooltip("Numero de Soldado que se quitara")]
    public int NumeroSoldado;
    ManejadorGeneralMundo MGM;

    //--------------

    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
    }

        /// <summary>
        /// Quitar el Soldado.
        /// </summary>
    public void Quitar()
    {
        string nombreIconoMision = MGM.MisionPanel.NombreMisionMapa;
        Mision ScriptMision = GameObject.Find(nombreIconoMision).GetComponent<Mision>();

        if (ScriptMision.EspacioTropas[NumeroSoldado])
        {
            //ActivarBotonDeSoldado:
            GameObject Soldado = ScriptMision.infoSoldadosDisponibles[NumeroSoldado].SoldadoEnLista;
            Soldado.GetComponent<Button>().interactable = true;
            Soldado.GetComponent<Soldados>().InfoActualSoldado.EnMision = false;
            GameObject Soldado2 = ScriptMision.infoSoldadosDisponibles[NumeroSoldado].SoldadoEnListaMision;
            Soldado2.GetComponent<Button>().interactable = true;
            Soldado2.GetComponent<Soldados>().InfoActualSoldado.EnMision = false;
            //EliminarInformaciónDeSoldado:
            ScriptMision.EliminarSoldado(NumeroSoldado);
            //Cambiar el color al cuadrado de la tropa (Cambiar para que se ponga un icono vacio)
            this.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Iconos/Interfaz/BloqueVacio");
            ScriptMision.ActualizarResultados();
        }
    }

}
