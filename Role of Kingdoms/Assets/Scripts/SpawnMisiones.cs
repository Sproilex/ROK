using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMisiones : MonoBehaviour {

    public float TiempoAntesDeSpawnear = 1;
    public EFormatoTiempo FormatoTiempoAntesDeSpawnear;
    private float ContadorTiempoSpawnear;

    public int CantidadMisionesDisponibles = 3;

    public List<GameObject> MisionesDisponibles = new List<GameObject>(1);

    private ManejadorGeneralMundo MGM;
    Dictionary<EDificultadMisiones, float> DificultadesDisponibles = new Dictionary<EDificultadMisiones, float>();
    public List<Mision> _MisionesBetadas = new List<Mision>();

    private bool _GenerarMision;
    private Transform Mapa;

    private float ContadorCadaMision;

    GameObject MisionLista;

    //Getters y Setters:

    public bool GenerarMision
    {
        get { return _GenerarMision; }
        set { _GenerarMision = value; if (value) StartCoroutine(SeleccionarMision()); }
    }

    public List<Mision> MisionesBetadas
    {
        get { return _MisionesBetadas; }
        set { _MisionesBetadas = value; }
    }

    //Metodos:

    void Start()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        Mapa = GameObject.FindGameObjectWithTag("Mapa").transform;
        ContadorTiempoSpawnear = ManejadorGeneralMundo.CambiarTiempoAFormato(FormatoTiempoAntesDeSpawnear,TiempoAntesDeSpawnear);
        for (int contador = 0; contador < MisionesDisponibles.Count; contador++)
        {
            GameObject Temp = Instantiate(MisionesDisponibles[contador],GameObject.Find("[InformacionMisiones]").transform);
            Temp.name = "[InformacionMision]" + Temp.name + "[Manejador:" + name + "]" + contador;
            Temp.GetComponent<Mision>().EsInformacion = true;
            MisionesDisponibles[contador] = Temp;
        }
        GenerarMision = true;
        ActualizarDificultadesDisponibles();
    }

    IEnumerator SeleccionarMision()
    {
        if(ContadorTiempoSpawnear > 0)
        {
            ContadorTiempoSpawnear -= 1;
        }
        else
        {
            //Seleccionar Dificultad Mision:
            float NumeroGenerado = Random.Range(0.0f, 100.0f);
            
            EDificultadMisiones DificultadSeleccionada = DevolverDificultadSeleccionada(NumeroGenerado);

            //Seleccionar Mision:
            if (DificultadSeleccionada != EDificultadMisiones.Nada)
            {
                List<GameObject> MisionesPreSeleccionadas = new List<GameObject>();

                for (int contador = 0; contador < MisionesDisponibles.Count; contador++)
                {
                    if (MisionesDisponibles[contador] != null)
                    {
                        Mision ScriptMision = MisionesDisponibles[contador].GetComponent<Mision>();
                        bool EsMisionBetada = MisionesBetadas.Find(x => x.Equals(ScriptMision)) != default(Mision);
                        if (ScriptMision.DificultadMision == DificultadSeleccionada && !EsMisionBetada)
                        {
                            MisionesPreSeleccionadas.Add(ScriptMision.gameObject);
                        }
                    }

                }

                ContadorTiempoSpawnear = ManejadorGeneralMundo.CambiarTiempoAFormato(FormatoTiempoAntesDeSpawnear, TiempoAntesDeSpawnear);


                if (MisionesPreSeleccionadas.Count > 0)
                {
                    int NumeroMisionSeleccionada = Random.Range(0, MisionesPreSeleccionadas.Count - 1);

                    MisionLista = MisionesPreSeleccionadas[NumeroMisionSeleccionada];

                    ContadorCadaMision = ManejadorGeneralMundo.CambiarTiempoAFormato(MisionLista.GetComponent<Mision>().FormatoTiempoParaRespawn, MisionLista.GetComponent<Mision>().TiempoParaRespawn);
                    StartCoroutine(ContarTiempoDeMision());
                    _GenerarMision = false;

                    Mision InfMision = MisionLista.GetComponent<Mision>();

                    if (InfMision.EsMisionUnica)
                    {
                        SpawnMisiones[] ManejadoresSpawn = GameObject.FindObjectsOfType<SpawnMisiones>();
                        foreach (SpawnMisiones CSpawn in ManejadoresSpawn)
                        {
                            CSpawn.MisionesBetadas.Add(InfMision);
                        }
                    }

                }
                else
                {
                    _GenerarMision = true;
                }
            }
            else
            {
                ContadorTiempoSpawnear = ManejadorGeneralMundo.CambiarTiempoAFormato(FormatoTiempoAntesDeSpawnear, TiempoAntesDeSpawnear);
            }
        }
        yield return new WaitForSeconds(1f);
        if(GenerarMision)
            StartCoroutine(SeleccionarMision());
    }

    EDificultadMisiones DevolverDificultadSeleccionada(float numeroGenerado)
    {
        Dictionary<EDificultadMisiones, float> DificultadesTomar = new Dictionary<EDificultadMisiones, float>();
        for(int contador = 0; contador < MisionesDisponibles.Count; contador++)
        {
            Mision MActual = MisionesDisponibles[contador].GetComponent<Mision>();
            if (_MisionesBetadas.Find(x => x.Equals(MActual)) == null &&
                !DificultadesTomar.ContainsKey(MActual.DificultadMision))
            {
                DificultadesTomar.Add(MActual.DificultadMision, MGM.DificultadesDisponibles[MActual.DificultadMision]);
            }
        }

        if(numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.Normal] && DificultadesTomar.ContainsKey(EDificultadMisiones.Normal))
        {
            return EDificultadMisiones.Normal;
        }else 
        if (numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.PocoNormal]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.Normal] 
            && DificultadesTomar.ContainsKey(EDificultadMisiones.PocoNormal))
        {
            return EDificultadMisiones.PocoNormal;
        }else
        if (numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.AlgoRaro]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.PocoNormal] 
            && DificultadesTomar.ContainsKey(EDificultadMisiones.AlgoRaro))
        {
            return EDificultadMisiones.AlgoRaro;
        }else 
        if (numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.BastanteRaro]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.AlgoRaro] 
            && DificultadesTomar.ContainsKey(EDificultadMisiones.BastanteRaro))
        {
            return EDificultadMisiones.BastanteRaro;
        }else
        if (numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.MuyRaro]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.BastanteRaro] 
            && DificultadesTomar.ContainsKey(EDificultadMisiones.MuyRaro))
        {
            return EDificultadMisiones.MuyRaro;
        }else
        if (numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.UltraRaro]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.MuyRaro] 
            && DificultadesTomar.ContainsKey(EDificultadMisiones.UltraRaro))
        {
            return EDificultadMisiones.UltraRaro;
        }else if (numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.Extremo]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.UltraRaro] 
            && DificultadesTomar.ContainsKey(EDificultadMisiones.Extremo))
        {
            return EDificultadMisiones.Extremo;
        }else 
        if (numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.MuyExtremo]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.Extremo] 
            && DificultadesTomar.ContainsKey(EDificultadMisiones.MuyExtremo))
        {
            return EDificultadMisiones.MuyExtremo;
        }else 
        if (numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.UltraExtremo]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.MuyExtremo] 
            && DificultadesTomar.ContainsKey(EDificultadMisiones.UltraExtremo))
        {
            return EDificultadMisiones.UltraExtremo;
        }
        else 
        if(numeroGenerado <= MGM.DificultadesDisponibles[EDificultadMisiones.CasiImposible]
            && numeroGenerado > MGM.DificultadesDisponibles[EDificultadMisiones.UltraExtremo]
            && DificultadesTomar.ContainsKey(EDificultadMisiones.UltraExtremo))
        {
            return EDificultadMisiones.CasiImposible;
        }
        else
        {
            return EDificultadMisiones.Nada;
        }
    }

    public void SpawnearMision()
    {
        Mision InfMision = MisionesDisponibles.Find(z => z == MisionLista).GetComponent<Mision>();
        GameObject MisionSpawneada = Instantiate(Resources.Load("Prefabs/Misiones/PrefabMisiones") as GameObject, Mapa);
        Mision ScriptMisionSpawneada = MisionSpawneada.GetComponent<Mision>();
        MisionSpawneada.transform.position = this.transform.position;
        ScriptMisionSpawneada.ActualizarInformacion(InfMision);
        ScriptMisionSpawneada.MisionOriginal = "[Mision]" + name;
        MisionSpawneada.name = ScriptMisionSpawneada.MisionOriginal;
        ScriptMisionSpawneada.EsInformacion = false;
        ScriptMisionSpawneada.PrefabMision = InfMision.gameObject;

        FiltroMisiones Filtro = FindObjectOfType<FiltroMisiones>();
        Filtro.ActualizarLevelsYAplicarFormato();

        bool Desac = GameObject.FindGameObjectWithTag("Interfaz_Soldados").GetComponent<Animator>().GetBool("Subir") ||
                       GameObject.FindGameObjectWithTag("Mision_Para_Enviar").GetComponent<Animator>().GetBool("Bajar") ||
                       GameObject.FindObjectOfType<ManejadorPanelInformacionMisiones>().GetComponent<Animator>().GetBool("Abajo") ||
                       Filtro.GetComponent<Animator>().GetBool("Izquierda");
        MisionSpawneada.GetComponent<BoxCollider2D>().enabled = !Desac;
        MGM.MisionesEnMapa.Add(ScriptMisionSpawneada);

    }

    IEnumerator ContarTiempoDeMision()
    {
        ContadorCadaMision -= 1;
        yield return new WaitForSeconds(1f);
        if (ContadorCadaMision > 0)
        {
            StartCoroutine(ContarTiempoDeMision());
        }
        else
        {
            SpawnearMision();
        }
    }

    void ActualizarDificultadesDisponibles()
    {
        DificultadesDisponibles.Clear();
        for (int contador = 0; contador < MisionesDisponibles.Count; contador++)
        {
            if (MisionesDisponibles[contador] != null)
            {
                MisionesDisponibles[contador].GetComponent<Mision>().ManejadorDeSpawn = this;
                MisionesDisponibles[contador].GetComponent<Mision>().PrefabMision = MisionesDisponibles[contador];

                EDificultadMisiones Llave = MisionesDisponibles[contador].GetComponent<Mision>().DificultadMision;
                if (!DificultadesDisponibles.ContainsKey(Llave))
                {
                    DificultadesDisponibles.Add(Llave, MGM.DificultadesDisponibles[Llave]);
                }
            }
        }
    }

}
