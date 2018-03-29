using UnityEngine;

public class SistemaConstruccionesCiudad : MonoBehaviour {

    ManejadorGeneralMundo MGM;
    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
    }

	public void CrearCuadricula()
    {
        Transform MapaCiudad = GameObject.FindGameObjectWithTag("Mapa_Sin_Misiones").transform;
        if (MapaCiudad.Find("Cuadricula").childCount == 0)
        {
            int tamanoCuadricula = MGM.TamanoCuadriculaConstrucciones;
            Transform posicionadorCuadriculas = MapaCiudad.Find("Posicionador_Cuadriculas");
            for (int n = 0; n < tamanoCuadricula; n++)
            {

                for (int m = 0; m < tamanoCuadricula; m++)
                {
                    string NumeroActual = (n).ToString() + (m).ToString();
                    if (MGM.NumeroCuadriculasBetadas.Find(x => x == NumeroActual) == null)
                    {
                        GameObject CuadriculaActual = Instantiate(Resources.Load("Prefabs/Construcciones_Ciudad/PrefabCuadricula"), posicionadorCuadriculas.position, posicionadorCuadriculas.rotation,
                                                              MapaCiudad.Find("Cuadricula")) as GameObject;
                        float x = tamanoCuadricula - n;
                        float y = tamanoCuadricula - m;
                        CuadriculaActual.transform.localPosition = new Vector3(CuadriculaActual.transform.localPosition.x + (1f * x),
                                                                               CuadriculaActual.transform.localPosition.y + (1f * y), -0.1f);
                        CuadriculaActual.name = "Cuadricula_Construcciones_" + n + "_" + m;
                    }
                }

            }
        }
    }
}
