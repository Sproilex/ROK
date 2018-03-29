using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporizadorTiempoEnMapaMisiones : MonoBehaviour {

    private Mision _Mision;

    public Mision Mision
    {
        get { return _Mision; }
        set { _Mision = value; }
    }

    void Update()
    {
        if (!Mision.SoldadoEnCamino)
        {
            StartCoroutine(_Mision.VerificarTiempoEnMapa());
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

}
