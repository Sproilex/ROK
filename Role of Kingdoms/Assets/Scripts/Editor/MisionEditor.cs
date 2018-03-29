using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Mision))]
[CanEditMultipleObjects]
public class MisionEditor : Editor {

    SerializedProperty EspacioTropas;
    SerializedProperty NombresEnemigos;
    SerializedProperty DificultadMision;
    SerializedProperty EsMisionUnica;
    SerializedProperty NombreBatalla;
    SerializedProperty DescripcionBatalla;
    SerializedProperty CantidadMinimaEnergiaRestarSoldados;
    SerializedProperty CantidadMaximaEnergiaRestarSoldados;
    SerializedProperty EXPADarASoldados;
    SerializedProperty IconoMision;

    SerializedProperty TiempoParaRespawn;
    SerializedProperty FormatoTiempoParaRespawn;

    SerializedProperty TiempoEnMapaBatalla;
    SerializedProperty FormatoTiempoEnMapaBatalla;

    SerializedProperty TiempoPeleaBatalla;
    SerializedProperty FormatoTiempoPeleaBatalla;

    ManejadorGeneralMundo MGM;
    bool DesplegarTiempos = false;

    void OnEnable()
    {
        MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();
        EspacioTropas = serializedObject.FindProperty("EspacioTropas");
        NombresEnemigos = serializedObject.FindProperty("NombresEnemigos");
        DificultadMision = serializedObject.FindProperty("DificultadMision");
        EsMisionUnica = serializedObject.FindProperty("EsMisionUnica");
        NombreBatalla = serializedObject.FindProperty("NombreBatalla");
        DescripcionBatalla = serializedObject.FindProperty("DescripcionBatalla");
        CantidadMinimaEnergiaRestarSoldados = serializedObject.FindProperty("CantidadMinimaEnergiaRestarSoldados");
        CantidadMaximaEnergiaRestarSoldados = serializedObject.FindProperty("CantidadMaximaEnergiaRestarSoldados");
        EXPADarASoldados = serializedObject.FindProperty("EXPADarASoldados");
        IconoMision = serializedObject.FindProperty("IconoMision");

        TiempoParaRespawn = serializedObject.FindProperty("TiempoParaRespawn");
        FormatoTiempoParaRespawn = serializedObject.FindProperty("FormatoTiempoParaRespawn");

        TiempoEnMapaBatalla = serializedObject.FindProperty("TiempoEnMapaBatalla");
        FormatoTiempoEnMapaBatalla = serializedObject.FindProperty("FormatoTiempoEnMapaBatalla");

        TiempoPeleaBatalla = serializedObject.FindProperty("TiempoPeleaBatalla");
        FormatoTiempoPeleaBatalla = serializedObject.FindProperty("FormatoTiempoPeleaBatalla");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Cantidad de Espacios para Soldados:", EditorStyles.boldLabel);
        EspacioTropas.arraySize = EditorGUILayout.IntSlider(EspacioTropas.arraySize, 1, 3);

        EditorGUILayout.LabelField("Informacion de los Enemigos: ", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Cantidad de Enemigos Disponibles ", EditorStyles.miniLabel);
        NombresEnemigos.arraySize = EditorGUILayout.IntSlider(NombresEnemigos.arraySize, 1, 3);
        for (int contador = 0; contador < NombresEnemigos.arraySize; contador++)
        {
            NombresEnemigos.GetArrayElementAtIndex(contador).stringValue = EditorGUILayout
                .TextField(new GUIContent("Nombre Enemigo " + (contador + 1) + ": "), NombresEnemigos.
                    GetArrayElementAtIndex(contador).stringValue);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Icono de la Mision: ", EditorStyles.miniLabel);
        EditorGUILayout.PropertyField(IconoMision);

        EditorGUILayout.LabelField("Informacion de la Batalla: ", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Dificultad: ", EditorStyles.miniLabel);
        DificultadMision.enumValueIndex = EditorGUILayout.Popup(DificultadMision.enumValueIndex, DificultadMision.enumNames);
        if (MGM.DificultadesDisponibles.Count > 0)
        {
            EDificultadMisiones DificultadSeleccionada;
            Enum.TryParse(Enum.GetName(typeof(EDificultadMisiones), DificultadMision.enumValueIndex), out DificultadSeleccionada);
            EditorGUILayout.LabelField("Porcentaje: " + MGM.DificultadesDisponibles[DificultadSeleccionada]);
        }
        else
        {
            EditorGUILayout.LabelField("No se ha podido cargar el porcentaje");
            MGM.CargarDatos();
        }
        

        EsMisionUnica.boolValue = EditorGUILayout.Toggle(new GUIContent("¿Es Mision Unica?"
            , "Si la mision aparecera solo una vez por el spawn que este colocado"), EsMisionUnica.boolValue);

        EditorGUILayout.LabelField("Nombre de Mision: ", EditorStyles.miniLabel);
        NombreBatalla.stringValue = EditorGUILayout.TextField(NombreBatalla.stringValue);

        EditorGUILayout.LabelField("Descripcion de Mision: ", EditorStyles.miniLabel);
        DescripcionBatalla.stringValue = EditorGUILayout.TextField(DescripcionBatalla.stringValue);
        EditorGUILayout.LabelField(DescripcionBatalla.stringValue.Length + "/40");
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Cantidad Minima de Energia para Soldados: ", EditorStyles.miniLabel);
        CantidadMinimaEnergiaRestarSoldados.intValue = EditorGUILayout.IntSlider(CantidadMinimaEnergiaRestarSoldados.intValue,
                                                                                 0, 100);
        EditorGUILayout.LabelField("Cantidad Maxima de Energia para Soldados: ", EditorStyles.miniLabel);
        CantidadMaximaEnergiaRestarSoldados.intValue = EditorGUILayout.IntSlider(CantidadMaximaEnergiaRestarSoldados.intValue,
                                                                                 0, 100);
        EditorGUILayout.LabelField("Cantidad de Experiencia a dar a los soldados: ", EditorStyles.miniLabel);
        EXPADarASoldados.intValue = EditorGUILayout.IntSlider(EXPADarASoldados.intValue,0,1000);

        //Tiempos:
        DesplegarTiempos = EditorGUILayout.Foldout(DesplegarTiempos, new GUIContent("Tiempos de Mision"));
        if (DesplegarTiempos)
        {
            EditorGUILayout.LabelField("Tiempo para Respawnear: ", EditorStyles.miniLabel);
            TiempoParaRespawn.intValue = EditorGUILayout.IntSlider(TiempoParaRespawn.intValue, 0, 60);
            FormatoTiempoParaRespawn.enumValueIndex = EditorGUILayout.Popup(FormatoTiempoParaRespawn.enumValueIndex,
                                                                            FormatoTiempoParaRespawn.enumNames);

            EditorGUILayout.LabelField("Tiempo que durara en el mapa: ", EditorStyles.miniLabel);
            TiempoEnMapaBatalla.intValue = EditorGUILayout.IntSlider(TiempoEnMapaBatalla.intValue, 0, 60);
            FormatoTiempoEnMapaBatalla.enumValueIndex = EditorGUILayout.Popup(FormatoTiempoEnMapaBatalla.enumValueIndex,
                                                                            FormatoTiempoEnMapaBatalla.enumNames);

            EditorGUILayout.LabelField("Tiempo de Pelea: ", EditorStyles.miniLabel);
            TiempoPeleaBatalla.intValue = EditorGUILayout.IntSlider(TiempoPeleaBatalla.intValue, 0, 60);
            FormatoTiempoPeleaBatalla.enumValueIndex = EditorGUILayout.Popup(FormatoTiempoPeleaBatalla.enumValueIndex,
                                                                            FormatoTiempoPeleaBatalla.enumNames);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
