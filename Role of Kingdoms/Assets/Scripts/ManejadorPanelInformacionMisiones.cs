using UnityEngine;

public class ManejadorPanelInformacionMisiones : MonoBehaviour {

    ManejadorGeneralMundo MGM;
    Animator _Animator;
    private int _ContadorActualMisiones = 0;

    public int ContadorActualMisiones
    {
        get { return _ContadorActualMisiones; }
        set { _ContadorActualMisiones = value; }
    }

    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        _Animator = this.GetComponent<Animator>();
    }

    public void OcultarDesplegarPanel(bool cerrar = false)
    {
        if (!cerrar)
        {
            bool Subir = _Animator.GetBool("Arriba");
            MGM.ActivarDesactivarSeleccionablesDelMapa(false, false, !Subir, false);
            MGM.ActivarDesactivarMovimientoMapa("Mapa_Principal", !Subir);
            _Animator.SetBool("Arriba", !Subir);
            _Animator.SetBool("Abajo", Subir);
        }
        else
        {
            _Animator.SetBool("Abajo", false);
            _Animator.SetBool("Arriba", true);
        }
    }

}
