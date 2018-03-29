using UnityEngine;
using UnityEngine.UI;

public class Check_Filtro : MonoBehaviour {

	private int _NumeroComienzoMisiones;
	private int _NumeroFinalMisiones;

    //Getters y Setters:
	public int NumeroComienzoMisiones{
		
		get{
			return _NumeroComienzoMisiones;
		}

		set{
			_NumeroComienzoMisiones = value;
		}

	}

	public int NumeroFinalMisiones{

		get{
			return _NumeroFinalMisiones;
		}

		set{
			_NumeroFinalMisiones = value;
		}

	}

    //------------------

    public void AlCambiarValor()
    {
        Mision[] Iconos_Misiones = GameObject.FindObjectsOfType(typeof(Mision)) as Mision[];
        foreach (Mision CSM in Iconos_Misiones)
        {
            if (CSM.LevelPromedio > (_NumeroFinalMisiones - 5) && CSM.LevelPromedio <= _NumeroFinalMisiones && !CSM.name.Contains("InformacionGuardada"))
            {
                bool activar = this.GetComponent<Toggle>().isOn;
                if (!CSM.SoldadoEnCamino)
                {
                    CSM.Ocultar(activar);
                }
            }

        }

    }

}
