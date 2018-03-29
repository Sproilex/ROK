using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnMisiones))]
[CanEditMultipleObjects]
public class SpawnMisionesEditor : Editor
{

    SerializedProperty TiempoAntesDeSpawnear;
    SerializedProperty FormatoTiempoAntesDeSpawnear;
    SerializedProperty CantidadMisiones;
    SerializedProperty MisionesDisponibles;

    void OnEnable()
    {
        TiempoAntesDeSpawnear = serializedObject.FindProperty("TiempoAntesDeSpawnear");
        FormatoTiempoAntesDeSpawnear = serializedObject.FindProperty("FormatoTiempoAntesDeSpawnear");
        CantidadMisiones = serializedObject.FindProperty("CantidadMisionesDisponibles");
        MisionesDisponibles = serializedObject.FindProperty("MisionesDisponibles");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField(new GUIContent("Tiempo Antes de Spawnear:", "Tiempo que pasara antes de que comience a contar el spawn de una mision"));
        EditorGUILayout.Slider(TiempoAntesDeSpawnear,1,60,new GUIContent(""));
        FormatoTiempoAntesDeSpawnear.enumValueIndex =
        EditorGUILayout.Popup(FormatoTiempoAntesDeSpawnear.enumValueIndex, FormatoTiempoAntesDeSpawnear.enumDisplayNames);

        EditorGUILayout.Space();
        EditorGUILayout.IntSlider(CantidadMisiones, 1, 100, new GUIContent("Cantidad de Misiones:"));
        MisionesDisponibles.arraySize = CantidadMisiones.intValue;
        for (int contador = 0; contador < CantidadMisiones.intValue; contador++)
        {
            SerializedProperty CObjetoMision = MisionesDisponibles.GetArrayElementAtIndex(contador);
            CObjetoMision.objectReferenceValue =
            EditorGUILayout.ObjectField(new GUIContent("Mision " + (contador + 1)), CObjetoMision.objectReferenceValue, CObjetoMision.objectReferenceValue.GetType(),false);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
