using UnityEngine;

public class ZoomCamara : MonoBehaviour {

    [Tooltip("Cantidad Minima de Zoom que se puede realizar")]
	public float MinimoZoom;
    [Tooltip("Cantidad Maxima de Zoom que se puede realizar")]
    public float MaximoZoom;
    [Tooltip("Velocidad a la que se realiza el zoom")]
    public float VelocidadZoom;

	private Camera Camara;

    /// <summary>
    /// Inicializacion de Variables
    /// </summary>
	void Start(){
		Camara = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
	}

    /// <summary>
    /// Realiza el zoom cuando se mueva la rueda del raton
    /// </summary>
	void Update(){

		float Rueda = Input.GetAxis ("Mouse ScrollWheel");
        if(Rueda != 0)
        {
            DesplazamientoMapas DM = GameObject.FindGameObjectWithTag(GameObject.Find("Mapa_Principal") != null ? "Mapa"
            : "Mapa_Sin_Misiones").GetComponent<DesplazamientoMapas>();

            if (Rueda > 0 && Camara.orthographicSize < MaximoZoom)
            {
                Camara.orthographicSize += Rueda + VelocidadZoom;
            }
            else if (Rueda < 0 && Camara.orthographicSize > MinimoZoom)
            {
                DM.MoverAlHacerZoom(0.5f);
                Camara.orthographicSize += Rueda + (VelocidadZoom * -1);
            }
        }
	}

}
