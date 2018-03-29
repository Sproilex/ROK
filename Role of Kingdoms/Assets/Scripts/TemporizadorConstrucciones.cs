using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TemporizadorConstrucciones : MonoBehaviour {

    private float _TiempoParaActivar;
    private float _ContadorTiempo;
    private Sprite _IconoCambiarAlTerminar;
    private string _nombreConstruccion;
    private bool objetoActivo;
    private GameObject _BarraRestanteTiempo;
    private Vector3 TamanoOriginalBarra;

    //Getters y Setters:
    public void TiempoParaActivar(EFormatoTiempo Formato, float Tiempo)
    {
        _TiempoParaActivar = ManejadorGeneralMundo.CambiarTiempoAFormato(Formato, Tiempo);
    }

    public Sprite IconoCambiarAlTerminar
    {
        get { return _IconoCambiarAlTerminar; }
        set { _IconoCambiarAlTerminar = value; }
    }

    public string NombreConstruccion
    {
        get { return _nombreConstruccion; }
        set { _nombreConstruccion = value; }
    }

    //Metodos:

    void Start()
    {
        _ContadorTiempo = _TiempoParaActivar;
        Transform PosicionadorBarra = GameObject.Find(_nombreConstruccion).transform.GetChild(0);
        Vector3 posBarra = Camera.main.WorldToScreenPoint(PosicionadorBarra.position);
        _BarraRestanteTiempo = Instantiate(Resources.Load("Prefabs/Construcciones_Ciudad/Progreso_Tiempo_Prefab") as GameObject,
                                           PosicionadorBarra);
        _BarraRestanteTiempo.transform.SetParent(GameObject.Find("Interfaz").transform.Find("Barras_De_Carga"));
        _BarraRestanteTiempo.transform.position = posBarra;
    }

    void Update()
    {
        StartCoroutine("VerificarSiConstruccionActiva");
        _ContadorTiempo -= Time.deltaTime;
        if(_ContadorTiempo <= 0 && objetoActivo)
        {
            GameObject Construccion = GameObject.Find(_nombreConstruccion);
            Construccion.GetComponent<EdificioCiudad>().Activo = true;
            Construccion.GetComponent<SpriteRenderer>().sprite = _IconoCambiarAlTerminar;
            Destroy(_BarraRestanteTiempo);
            Destroy(this.gameObject);
        }
    }

    IEnumerator VerificarSiConstruccionActiva()
    {
        if (GameObject.Find(_nombreConstruccion) != null)
        {
            if (_BarraRestanteTiempo != null)
            {
                _BarraRestanteTiempo.SetActive(true);
            }
            Transform PosicionadorBarra = GameObject.Find(_nombreConstruccion).transform.GetChild(0);
            if (TamanoOriginalBarra == default(Vector3))
            {
                TamanoOriginalBarra = _BarraRestanteTiempo.transform.localScale;
            }
            float CValor = Camera.main.orthographicSize;
            _BarraRestanteTiempo.transform.localScale = new Vector3((TamanoOriginalBarra.x * 100) / CValor, (TamanoOriginalBarra.y * 100) / CValor, 0) / 10;

            _BarraRestanteTiempo.transform.position = Camera.main.WorldToScreenPoint(PosicionadorBarra.position);
            int porcentajeTiempo = 100 - (int)Mathf.Abs((_ContadorTiempo * 100) / _TiempoParaActivar);
            _BarraRestanteTiempo.GetComponent<Slider>().value = porcentajeTiempo;
            objetoActivo = true;
        }
        else
        {
            _BarraRestanteTiempo.SetActive(false);
            objetoActivo = false;
        }
        yield return new WaitForSeconds(.5f);
    }

}
