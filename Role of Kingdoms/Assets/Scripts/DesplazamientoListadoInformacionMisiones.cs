using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DesplazamientoListadoInformacionMisiones : MonoBehaviour
{
    private Transform PosAgrupador;
    private Transform PosMisionesOcultas;
    public List<GameObject> _Misiones = new List<GameObject>();

    public List<GameObject> Misiones
    {
        get { return _Misiones; }
        set { _Misiones = value; }
    }

    void Start()
    {
        PosAgrupador = GameObject.FindGameObjectWithTag("Agrupador_Panel_Informacion_Misiones").transform;
        PosMisionesOcultas = PosAgrupador.Find("Espacio_Misiones_Ocultas");
    }

	public void Subir()
    {
        GameObject[] MisionesVistas = new GameObject[3];
        int NumeroMisionesActuales = _Misiones.Count;
        int[] PosMisionesEnList = new int[] { NumeroMisionesActuales, NumeroMisionesActuales, -NumeroMisionesActuales };
        for (int contador = 0; contador < MisionesVistas.Length; contador++)
        {
            Transform PosPadreMision = PosAgrupador.Find("Espacio_Informacion_Mision_" + (contador + 1));
            if (PosPadreMision.childCount > 0)
            {
                MisionesVistas[contador] = PosPadreMision.GetChild(0).gameObject;

                for (int contador2 = 0; contador2 < _Misiones.Count; contador2++)
                {
                    if (_Misiones[contador2] == MisionesVistas[contador])
                    {
                        PosMisionesEnList[contador] = contador2;
                        break;
                    }
                }

            }
        }

        //Mover:

        if (MisionesVistas[0] != null && PosMisionesEnList[0] - 1 >= 0)
        {
            _Misiones[PosMisionesEnList[0] - 1].transform.SetParent(MisionesVistas[0].transform.parent);
        }

        if (MisionesVistas[0] != null)
        {
            MisionesVistas[0].transform.SetParent(PosAgrupador.Find("Espacio_Informacion_Mision_2"));
        }

        if (MisionesVistas[1] != null)
        {
            MisionesVistas[1].transform.SetParent(PosAgrupador.Find("Espacio_Informacion_Mision_3"));
        }

        if (MisionesVistas[2] != null)
        {
            MisionesVistas[2].transform.SetParent(PosMisionesOcultas);
        }
        //Debug.Log("Ultimo Numero: " + PosMisionesEnList[2] + "   Suma de ultimo numero mas 2: " + (PosMisionesEnList[2] + 2) + "   Numero de Misiones Total: " + _Misiones.Count);

        int ultimaMisionActiva = PosMisionesEnList[0] != NumeroMisionesActuales ? PosMisionesEnList[0] :
                                 PosMisionesEnList[1] != NumeroMisionesActuales ? PosMisionesEnList[1] :
                                 PosMisionesEnList[2] != NumeroMisionesActuales ? PosMisionesEnList[2] : NumeroMisionesActuales;


        if (ultimaMisionActiva - 2 < 0)
        {
            this.GetComponent<Button>().interactable = false;
        }

        if (PosMisionesEnList[1] + 1 >= _Misiones.Count)
        {
            transform.parent.Find("Boton_Bajar").GetComponent<Button>().interactable = false;
        }
        else
        {
            transform.parent.Find("Boton_Bajar").GetComponent<Button>().interactable = true;
        }

    }

    public void Bajar()
    {
        GameObject[] MisionesVistas = new GameObject[3];
        int[] PosMisionesEnList = new int[] { -1,-1,-1};
        for (int contador = 0; contador < MisionesVistas.Length; contador++)
        {
            Transform PosPadreMision = PosAgrupador.Find("Espacio_Informacion_Mision_" + (contador + 1));
            if (PosPadreMision.childCount > 0)
            {
                MisionesVistas[contador] = PosPadreMision.GetChild(0).gameObject;

                for (int contador2 = 0; contador2 < _Misiones.Count; contador2++)
                {
                    if (_Misiones[contador2] == MisionesVistas[contador])
                    {
                        PosMisionesEnList[contador] = contador2;
                        break;
                    }
                }

            }
        }

        //Mover:

        if (MisionesVistas[2] != null && PosMisionesEnList[2] + 1 < _Misiones.Count)
        {
            _Misiones[PosMisionesEnList[2] + 1].transform.SetParent(MisionesVistas[2].transform.parent);
        }

        if (MisionesVistas[2] != null)
        {
            MisionesVistas[2].transform.SetParent(PosAgrupador.Find("Espacio_Informacion_Mision_2"));
        }

        if (MisionesVistas[1] != null)
        {
            MisionesVistas[1].transform.SetParent(PosAgrupador.Find("Espacio_Informacion_Mision_1"));
        }

        if (MisionesVistas[0] != null)
        {
            MisionesVistas[0].transform.SetParent(PosMisionesOcultas);
        }
        //Debug.Log("Ultimo Numero: " + PosMisionesEnList[2] + "   Suma de ultimo numero mas 2: " + (PosMisionesEnList[2] + 2) + "   Numero de Misiones Total: " + _Misiones.Count);

        int ultimaMisionActiva = PosMisionesEnList[2] != -1 ? PosMisionesEnList[2] : 
                                 PosMisionesEnList[1] != -1 ? PosMisionesEnList[1] : 
                                 PosMisionesEnList[0] != -1 ? PosMisionesEnList[0] : _Misiones.Count;
        if (ultimaMisionActiva + 2 >= _Misiones.Count)
        {
            this.GetComponent<Button>().interactable = false;
        }

        if (PosMisionesEnList[1] - 1 < 0)
        {
            transform.parent.Find("Boton_Subir").GetComponent<Button>().interactable = false;
        }
        else
        {
            transform.parent.Find("Boton_Subir").GetComponent<Button>().interactable = true;
        }

    }

    public void EliminarDeLista(GameObject MisionInformacionAEliminar)
    {
        int NumeroMisionEliminar = 0;

        for (int contadorM = 0; contadorM < _Misiones.Count; contadorM++)
        {
            if (_Misiones[contadorM].name == MisionInformacionAEliminar.name)
            {
                NumeroMisionEliminar = contadorM;
                break;
            }
        }

        int CantidadEspaciosMover = MisionInformacionAEliminar.transform.parent.name.Contains("1") ? 3 :
                                    MisionInformacionAEliminar.transform.parent.name.Contains("2") ? 2 : 1;

        int NumeroPadreInicial = 0;

        foreach (char CLetra in MisionInformacionAEliminar.transform.parent.name)
        {
            if (char.IsNumber(CLetra))
            {
                NumeroPadreInicial = Convert.ToInt32(CLetra.ToString());
                break;
            }
        }
        int UltimaMisionTomada = NumeroMisionEliminar;

        for(int contador = 0; contador < CantidadEspaciosMover; contador++)
        {
            Transform Padre = PosAgrupador.Find("Espacio_Informacion_Mision_" + (NumeroPadreInicial + contador));
            if (NumeroMisionEliminar + (contador + 1) < _Misiones.Count)
            {
                GameObject MisionAMover = _Misiones[NumeroMisionEliminar + (contador + 1)];
                if (MisionAMover != null)
                {
                    MisionAMover.transform.SetParent(Padre);
                    UltimaMisionTomada = NumeroMisionEliminar + (contador + 1);
                }
            }
        }

        if(UltimaMisionTomada + 1 >= _Misiones.Count)
        {
            transform.parent.Find("Boton_Bajar").GetComponent<Button>().interactable = false;
        }

        _Misiones.Remove(MisionInformacionAEliminar);
    }

}
