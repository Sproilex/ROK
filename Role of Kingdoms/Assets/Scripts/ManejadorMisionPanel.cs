using UnityEngine;
using UnityEngine.UI;

public class ManejadorMisionPanel : MonoBehaviour {

    private Animator _animatorMMP;
    private string _nombreMisionMapa;
    private Text _TextoPorcentaje;
    private GameObject[] _IconosEnemigos = new GameObject[3];
    private GameObject[] _IconosTropas = new GameObject[3];

    //Getters y Setters:

    public string NombreMisionMapa
    {
        get { return _nombreMisionMapa; }
        set { _nombreMisionMapa = value; }
    }

    /// <summary>
    /// Inicializacion de Variables
    /// </summary>
    void Start()
    {
        _animatorMMP = gameObject.GetComponent<Animator>();
        Transform PosElementos = transform.Find("Detalles_Mision").Find("Panel_Informacion_Y_Enemigos").Find("Elementos_Mision");
        _TextoPorcentaje = PosElementos.Find("Probabilidad_Victoria").GetComponent<Text>();
        for (int n = 0; n < 3; n++)
        {
            _IconosTropas[n] = PosElementos.Find("Espacio_Soldado_" + (n + 1)).gameObject;
            _IconosEnemigos[n] = PosElementos.Find("Enemigo_" + (n + 1)).gameObject;
        }
    }

    public void DesplegarOcultarPanel()
    {
        if (_animatorMMP.GetBool("Bajar") == false && _animatorMMP.GetBool("Subir") == false)
        {
            _animatorMMP.SetBool("Bajar", true);
        }
        else
        {
            _animatorMMP.SetBool("Subir", !_animatorMMP.GetBool("Subir"));
            _animatorMMP.SetBool("Bajar", !_animatorMMP.GetBool("Bajar"));
        }
    }

    public void resetearDatos()
    {
        _TextoPorcentaje.text = "0 % victoria";
        for (int n = 0; n < 3; n++)
        {
            _IconosTropas[n].SetActive(true);
            _IconosTropas[n].GetComponent<Image>().sprite = Resources.Load<Sprite>("Iconos/Interfaz/BloqueVacio");
            _IconosTropas[n].GetComponent<Image>().color = new Color(1,1,1,1);
            _IconosEnemigos[n].SetActive(true);

            Transform PosElementos = transform.Find("Detalles_Mision").Find("Panel_Informacion_Y_Enemigos").Find("Elementos_Mision");
            GameObject Borde = PosElementos.parent.parent.Find("Panel_Lista_Tropas").Find("Borde_Seleccion_Carta_" + (n + 1)).gameObject;
            Color CActual = Borde.GetComponent<Image>().color;
            Borde.GetComponent<Image>().color = new Color(CActual.r, CActual.g, CActual.b, 0);
        }

    }
}
