using UnityEngine;

public class ManejadorPanelInformacionCuartelSoldados : MonoBehaviour
{
    private Animator _Animator;
    private ManejadorGeneralMundo MGM;

    void Start()
    {
        _Animator = this.GetComponent<Animator>();
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
    }

    public void DesplegarOcultarPanel(bool Cerrar = false)
    {
        if (!Cerrar)
        {
            if (_Animator.GetBool("Bajar") == false && _Animator.GetBool("Subir") == false)
            {
                MGM.ActivarDesactivarSeleccionablesDelMapa(false,false,false,false);
                MGM.ActivarDesactivarMovimientoMapa("Ciudad",false);
                _Animator.SetBool("Subir", true);
            }
            else
            {
                bool Subir = !_Animator.GetBool("Subir");
                MGM.ActivarDesactivarSeleccionablesDelMapa(false,false,!Subir,false);
                MGM.ActivarDesactivarMovimientoMapa("Ciudad",!Subir);
                _Animator.SetBool("Subir", Subir);
                _Animator.SetBool("Bajar", !Subir);
            }
            
        }
        else
        {
            _Animator.SetBool("Subir", false);
            _Animator.SetBool("Bajar", true);
        }

    }

}
