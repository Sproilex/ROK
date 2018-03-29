using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorDetallesPanelMision : MonoBehaviour {

    //Informacion de Textos de la Informacion:
    private Text NombreSoldado;
    private Text LevelSoldado;
    private Text VidaSoldado;
    private Text DañoSoldado;
    private Text EnergiaSoldado;
    private Text ExperienciaSoldado;
    private Text PoderesSoldado;
    private Text HeroeSoldado;
    private GameObject EspacioPoder1;
    private GameObject EspacioPoder2;
    private GameObject EspacioPoder3;
    //Falta agregar casillas de poderes y los modificadores de atributos


    void Start()
    {
        NombreSoldado = transform.Find("Nombre_Soldado").GetComponent<Text>();
        LevelSoldado = transform.Find("Level_Soldado").GetComponent<Text>();
        VidaSoldado = transform.Find("Vida_Soldado").GetComponent<Text>();
        DañoSoldado = transform.Find("Daño_Soldado").GetComponent<Text>();
        EnergiaSoldado = transform.Find("Energia_Soldado").GetComponent<Text>();
        ExperienciaSoldado = transform.Find("Experiencia_Soldado").GetComponent<Text>();
        PoderesSoldado = transform.Find("Poderes_Soldado").GetComponent<Text>();
        HeroeSoldado = transform.Find("Heroe_Soldado").GetComponent<Text>();
        EspacioPoder1 = transform.Find("Casilla_Poder_1").gameObject;
        EspacioPoder2 = transform.Find("Casilla_Poder_2").gameObject;
        EspacioPoder3 = transform.Find("Casilla_Poder_3").gameObject;
        ResetearDatos();
    }

    public void CambiarDatos(__informacionSoldados infoColocar)
    {
        NombreSoldado.text = infoColocar.Nombre + " (" + infoColocar.Raza + " el " + infoColocar.Clase + ")";
        LevelSoldado.text = "Level : " + infoColocar.Nivel;
        VidaSoldado.text = "Vida : " + infoColocar.Vida;
        DañoSoldado.text = "Daño : " + infoColocar.Daño;
        EnergiaSoldado.text = "Energia : " + infoColocar.Energia;
        ExperienciaSoldado.text = "Experiencia : " + infoColocar.EXP + "/" + infoColocar.EXPAL;
        PoderesSoldado.text = "Poderes : " + infoColocar.Poderes + "/3";
        HeroeSoldado.text = (infoColocar.Heroe ? "Si" : "No") + " es héroe";
    }

    public void CambiarDatos(__informacionEnemigos infoColocar)
    {
        NombreSoldado.text = infoColocar.Nombre;
        LevelSoldado.text = "Level : " + infoColocar.Level;
        VidaSoldado.text = "Vida : " + infoColocar.Vida;
        DañoSoldado.text = "Daño : " + infoColocar.Daño;
        EnergiaSoldado.text = "Energia : " + infoColocar.Energia;
        ExperienciaSoldado.text = "";
        PoderesSoldado.text = "Poderes : " + infoColocar.Poderes + "/3";
        HeroeSoldado.text = string.Empty;
    }

    public void ResetearDatos()
    {
        NombreSoldado.text = "Nadie Seleccionado";
        LevelSoldado.text = "Level :";
        VidaSoldado.text = "Vida :";
        DañoSoldado.text = "Daño :";
        EnergiaSoldado.text = "Energia :";
        ExperienciaSoldado.text = "Experiencia :";
        PoderesSoldado.text = "Poderes: 0/3";
        HeroeSoldado.text = "Nada Seleccionado";
        Sprite SlotVacio = Resources.Load<Sprite>("Iconos/Interfaz/BloqueVacio");
        EspacioPoder1.GetComponent<Image>().sprite = SlotVacio;
        EspacioPoder2.GetComponent<Image>().sprite = SlotVacio;
        EspacioPoder3.GetComponent<Image>().sprite = SlotVacio;
    }
}
