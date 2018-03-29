using UnityEngine;

public class DesplazamientoMapas : MonoBehaviour {

	private Vector3 Origen;
    [Tooltip("Posicion Maxima a la que llega el mapa hacia la izquierda")]
	public float PosicionMaximaIz;
    [Tooltip("Posicion Maxima a la que llega el mapa hacia la derecha")]
    public float PosicionMaximaDe;
    [Tooltip("Posicion Maxima a la que llega el mapa hacia arriba")]
    public float PosicionMaximaAr;
    [Tooltip("Posicion Maxima a la que llega el mapa hacia abajo")]
    public float PosicionMaximaAb;
    private bool _NoMover;

   public bool NoMover
    {
        get { return _NoMover; }
        set { _NoMover = value; }
    }

    /// <summary>
    ///     Coloca la posicion inicia en la que estaba el mapa.
    /// </summary>
    void OnMouseDown(){
        Origen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// Actualiza la posicion del mapa verificando las posiciones maximas
    /// </summary>
	void OnMouseDrag(){
        if (!_NoMover)
        {
            Vector3 PosicionMA = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
            Vector3 PosicionTotal = Origen - (Camera.main.ScreenToWorldPoint(PosicionMA) - Camera.main.transform.position);
            PosicionTotal.z = 20;

            PosicionTotal.x = PosicionTotal.x >= PosicionMaximaIz ? PosicionMaximaIz : PosicionTotal.x;
            PosicionTotal.x = PosicionTotal.x <= PosicionMaximaDe ? PosicionMaximaDe : PosicionTotal.x;
            PosicionTotal.y = PosicionTotal.y >= PosicionMaximaAb ? PosicionMaximaAb : PosicionTotal.y;
            PosicionTotal.y = PosicionTotal.y <= PosicionMaximaAr ? PosicionMaximaAr : PosicionTotal.y;

            Transform TCamara = Camera.main.transform;
            TCamara.position = PosicionTotal;
            TCamara.position = new Vector3(TCamara.position.x, TCamara.position.y, -10);

        }
    }

    public void MoverAlHacerZoom(float velocidadMovimiento)
    {
        Vector3 PosicionMA = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
        Vector3 PosicionTotal = Camera.main.ScreenToWorldPoint(PosicionMA) - Camera.main.transform.position;
        PosicionTotal.z = 20;

        PosicionTotal.x = PosicionTotal.x >= PosicionMaximaIz ? PosicionMaximaIz : PosicionTotal.x;
        PosicionTotal.x = PosicionTotal.x <= PosicionMaximaDe ? PosicionMaximaDe : PosicionTotal.x;
        PosicionTotal.y = PosicionTotal.y >= PosicionMaximaAb ? PosicionMaximaAb : PosicionTotal.y;
        PosicionTotal.y = PosicionTotal.y <= PosicionMaximaAr ? PosicionMaximaAr : PosicionTotal.y;

        Transform TCamara = Camera.main.transform;
        TCamara.Translate(PosicionTotal * velocidadMovimiento);
        TCamara.position = new Vector3(TCamara.position.x, TCamara.position.y, -10);
    }

}
