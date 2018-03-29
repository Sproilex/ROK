using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DesplazamientoListadoTropasMision : MonoBehaviour {

    private List<GameObject> _Soldados = new List<GameObject>();
    private List<GameObject> _SoldadosMision = new List<GameObject>();
    private Transform PosAgrupador;
    private Transform PosOcultos;
    private string NombreEspacioCartas;
    [Tooltip("Si se quieren mover las tropas del panel de la lista de soldados (Dejar desmarcado si es el panel de misiones)")]
    public bool EsPanelListaSoldados;

    public List<GameObject> Soldados
    {
        get { return _Soldados; }
        set { _Soldados = value; }
    }

    public List<GameObject> SoldadosMision
    {
        get { return _SoldadosMision; }
        set { _SoldadosMision = value; }
    }

    void Start()
    {
        if (!EsPanelListaSoldados)
        {
            PosAgrupador = GameObject.FindGameObjectWithTag("Mision_Para_Enviar").transform.Find("Detalles_Mision").Find("Panel_Lista_Tropas");
            NombreEspacioCartas = "Borde_Seleccion_Carta_";
            PosOcultos = PosAgrupador.Find("Espacio_Ocultos");
        }
        else
        {
            PosAgrupador = GameObject.FindGameObjectWithTag("Soldados_Listados").transform;
            PosOcultos = PosAgrupador.parent.Find("Espacio_Ocultos");
            NombreEspacioCartas = "Espacio_Soldado_";
        }
    }

	public void DesplazarIzquierda()
    {
        if (!EsPanelListaSoldados)
        {
            DeseleccionarSoldadosYLimpiar();
        }

        GameObject[] EspaciosVistos = new GameObject[5];
        int[] PosSoldadosEnList = new int[5];
        for (int contador = 0; contador < EspaciosVistos.Length; contador++)
        {
            Transform PosPadreMision = PosAgrupador.Find(NombreEspacioCartas + (contador + 1));
            if (PosPadreMision.childCount > 0)
            {
                EspaciosVistos[contador] = PosPadreMision.GetChild(0).gameObject;

                for (int contador2 = 0; contador2 < _Soldados.Count; contador2++)
                {
                    if (_Soldados[contador2].name == EspaciosVistos[contador].name)
                    {
                        PosSoldadosEnList[contador] = contador2;
                        break;
                    }
                }

            }
        }
        transform.parent.Find("Flecha_Derecha").GetComponent<Button>().interactable = true;
        if (EspaciosVistos[0] != null && PosSoldadosEnList[0] - 1 >= 0)
        {
            
            GameObject SoldadoMover = PosOcultos.Find((EsPanelListaSoldados ? _Soldados : _SoldadosMision)[PosSoldadosEnList[0] - 1].name).gameObject;
            SoldadoMover.transform.SetParent(EspaciosVistos[0].transform.parent);
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[0] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[0];
            SoldadoMover.transform.SetParent(PosAgrupador.Find(NombreEspacioCartas + "2"));
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[1] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[1];
            SoldadoMover.transform.SetParent(PosAgrupador.Find(NombreEspacioCartas + "3"));
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[2] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[2];
            SoldadoMover.transform.SetParent(PosAgrupador.Find(NombreEspacioCartas + "4"));
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[3] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[3];
            SoldadoMover.transform.SetParent(PosAgrupador.Find(NombreEspacioCartas + "5"));
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[4] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[4];
            SoldadoMover.transform.SetParent(PosOcultos);
            SoldadoMover.transform.position = SoldadoMover.transform.parent.position;
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (PosSoldadosEnList[0] - 2 < 0)
        {
            this.GetComponent<Button>().interactable = false;
        }

    }

    public void DesplazarDerecha()
    {
        if (!EsPanelListaSoldados)
        {
            DeseleccionarSoldadosYLimpiar();
        }

        GameObject[] EspaciosVistos = new GameObject[5];
        int[] PosSoldadosEnList = new int[5];
        for (int contador = 0; contador < EspaciosVistos.Length; contador++)
        {
            Transform PosPadreMision = PosAgrupador.Find(NombreEspacioCartas + (contador + 1));
            if (PosPadreMision.childCount > 0)
            {
                EspaciosVistos[contador] = PosPadreMision.GetChild(0).gameObject;

                for (int contador2 = 0; contador2 < _Soldados.Count; contador2++)
                {
                    if (_Soldados[contador2].name == EspaciosVistos[contador].name)
                    {
                        PosSoldadosEnList[contador] = contador2;
                        break;
                    }
                }

            }
        }

        transform.parent.Find("Flecha_Izquierda").GetComponent<Button>().interactable = true;
        if (EspaciosVistos[4] != null && PosSoldadosEnList[4] + 1 < (EsPanelListaSoldados ? _Soldados.Count : _SoldadosMision.Count))
        {
            GameObject SoldadoMover = (EsPanelListaSoldados ? _Soldados : _SoldadosMision)[PosSoldadosEnList[4] + 1];
            SoldadoMover.transform.SetParent(EspaciosVistos[4].transform.parent);
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[4] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[4];
            SoldadoMover.transform.SetParent(PosAgrupador.Find(NombreEspacioCartas + "4"));
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[3] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[3];
            SoldadoMover.transform.SetParent(PosAgrupador.Find(NombreEspacioCartas + "3"));
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[2] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[2];
            SoldadoMover.transform.SetParent(PosAgrupador.Find(NombreEspacioCartas + "2"));
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[1] != null)
        {
            GameObject SoldadoMover = EspaciosVistos[1];
            SoldadoMover.transform.SetParent(PosAgrupador.Find(NombreEspacioCartas + "1"));
            if (!EsPanelListaSoldados)
            {
                SoldadoMover.GetComponent<Soldados>().PadreOriginal = SoldadoMover.transform.parent;
            }
        }

        if (EspaciosVistos[0] != null)
        {
            EspaciosVistos[0].transform.SetParent(PosOcultos);
            EspaciosVistos[0].transform.position = EspaciosVistos[0].transform.parent.position;
        }

        if (PosSoldadosEnList[4] + 2 >= _Soldados.Count)
        {
            this.GetComponent<Button>().interactable = false;
        }

    }

    void DeseleccionarSoldadosYLimpiar()
    {
        Soldados[] Solds = GameObject.FindObjectsOfType<Soldados>();
        foreach (Soldados CSoldado in Solds)
        {
            if (CSoldado.PosibleEnviarAMision)
            {
                break;
            }
        }
        GameObject.FindObjectOfType<ManejadorDetallesPanelMision>().ResetearDatos();
    }

    public void EliminarSoldadoLista(GameObject SoldadoEliminar)
    {
        int NumeroSoldadoEliminar = 0;

        for (int contadorM = 0; contadorM < _Soldados.Count; contadorM++)
        {
            if (_Soldados[contadorM].name == SoldadoEliminar.name)
            {
                NumeroSoldadoEliminar = contadorM;
                break;
            }
        }

        int CantidadEspaciosMover = SoldadoEliminar.transform.parent.name.Contains("1") ? 5 :
                                    SoldadoEliminar.transform.parent.name.Contains("2") ? 4 :
                                    SoldadoEliminar.transform.parent.name.Contains("3") ? 3 :
                                    SoldadoEliminar.transform.parent.name.Contains("4") ? 2 : 1;

        int NumeroPadreInicial = 0;
        int UltimoSoldadoTomado = NumeroSoldadoEliminar;

        foreach (char CLetra in SoldadoEliminar.transform.parent.name)
        {
            if (char.IsNumber(CLetra))
            {
                NumeroPadreInicial = Convert.ToInt32(CLetra.ToString());
                break;
            }
        }
        for (int contador = 0; contador < CantidadEspaciosMover; contador++)
        {
            Transform Padre = PosAgrupador.Find(NombreEspacioCartas + (NumeroPadreInicial + contador));
            
            if (NumeroSoldadoEliminar + (contador + 1) < _Soldados.Count)
            {
                GameObject MisionAMover = _Soldados[NumeroSoldadoEliminar + (contador + 1)];
                if (MisionAMover != null)
                {
                    MisionAMover.transform.SetParent(Padre);
                    UltimoSoldadoTomado = NumeroSoldadoEliminar + (contador + 1);
                }
            }
        }

        if (UltimoSoldadoTomado + 1 >= _Soldados.Count)
        {
            transform.parent.Find("Flecha_Derecha").GetComponent<Button>().interactable = false;
        }

        _Soldados.Remove(SoldadoEliminar);

    }

    public void EliminarSoldadoMision(GameObject SoldadoEliminar)
    {
        int NumeroSoldadoEliminar = 0;

        for (int contadorM = 0; contadorM < _SoldadosMision.Count; contadorM++)
        {
            if (_SoldadosMision[contadorM].name == SoldadoEliminar.name)
            {
                NumeroSoldadoEliminar = contadorM;
                break;
            }
        }

        int CantidadEspaciosMover = SoldadoEliminar.transform.parent.name.Contains("1") ? 5 :
                                    SoldadoEliminar.transform.parent.name.Contains("2") ? 4 :
                                    SoldadoEliminar.transform.parent.name.Contains("3") ? 3 :
                                    SoldadoEliminar.transform.parent.name.Contains("4") ? 2 : 1;

        int NumeroPadreInicial = 0;
        int UltimoSoldadoTomado = NumeroSoldadoEliminar;

        foreach (char CLetra in SoldadoEliminar.transform.parent.name)
        {
            if (char.IsNumber(CLetra))
            {
                NumeroPadreInicial = Convert.ToInt32(CLetra.ToString());
                break;
            }
        }

        for (int contador = 0; contador < CantidadEspaciosMover; contador++)
        {
            Transform Padre = PosAgrupador.Find(NombreEspacioCartas + (NumeroPadreInicial + contador));

            if (NumeroSoldadoEliminar + (contador + 1) < _SoldadosMision.Count)
            {
                GameObject MisionAMover = _SoldadosMision[NumeroSoldadoEliminar + (contador + 1)];
                if (MisionAMover != null)
                {
                    MisionAMover.transform.SetParent(Padre);
                    UltimoSoldadoTomado = NumeroSoldadoEliminar + (contador + 1);
                }
            }
        }

        if (UltimoSoldadoTomado + 1 >= _SoldadosMision.Count)
        {
            transform.parent.Find("Flecha_Derecha").GetComponent<Button>().interactable = false;
        }

        Soldados[] SoldadosA = GameObject.FindObjectsOfType<Soldados>();

        foreach(Soldados CSoldado in SoldadosA)
        {
            if(CSoldado.PosibleEnviarAMision && CSoldado.gameObject.activeInHierarchy)
            {
                CSoldado.PadreOriginal = CSoldado.transform.parent;
            }
        }

        _SoldadosMision.Remove(SoldadoEliminar);


    }

}
