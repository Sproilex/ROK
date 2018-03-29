using System.Collections;
using UnityEngine;

public class TemporizadorTranscursoMisiones : MonoBehaviour {

    private float _TiempoContador;
    private float _TiempoTotal;
    private float _TiempoCombate;
    public MisionInformacionManejador MIMActual;
    private LineRenderer _LineaSeguimiento;
    bool _BajarTiempo;
    bool YaPasoCombate;

    //Getters y Setters:

    public LineRenderer LineaSeguimiento
    {
        get { return _LineaSeguimiento; }
        set { _LineaSeguimiento = value; }
    }

    public float TiempoTotal
    {
        get { return _TiempoTotal; }
        set { _TiempoTotal = value; }
    }

    public bool BajarTiempo
    {
        get { return _BajarTiempo; }
        set { _BajarTiempo = value; }
    }

    public float TiempoTotalCombate
    {
        get { return _TiempoCombate; }
        set { _TiempoCombate = value; }
    }

    //Metodos:

    void Start()
    {
        _TiempoContador = _TiempoTotal;
        StartCoroutine(BajarTiempoYAvisar());
    }

    void Update()
    {
        StartCoroutine(BajarTiempoYAvisar());
    }

    IEnumerator BajarTiempoYAvisar()
    {
        if(_TiempoContador <= 0)
        {
            MIMActual.CalcularResultado();
            _BajarTiempo = false;
            _TiempoContador = _TiempoTotal;
            Destroy(gameObject);
        }
        else
        {
            if(_TiempoContador <= (_TiempoTotal / 2) && _TiempoCombate > 0)
            {
                MIMActual.ManejarBarra();
                MIMActual.UtilizarTiempoTotal = false;
                _TiempoCombate -= Time.deltaTime;
                MIMActual.TiempoCombate = _TiempoCombate;
                MIMActual.VerificacionesMovimiento(_TiempoContador);
                YaPasoCombate = true;
            }
            else
            {
                if (YaPasoCombate)
                {
                    LineaSeguimiento.SetPosition(1, MIMActual.transform.position);
                }
                MIMActual.TiempoCombate = 0;
                MIMActual.TiempoRestanteMovimiento += Time.deltaTime / (_TiempoTotal / 2);
                _TiempoContador -= Time.deltaTime;
                MIMActual.VerificacionesMovimiento(_TiempoContador);
            }
            
        }

        yield return new WaitForSeconds(.5f);
    }

}
