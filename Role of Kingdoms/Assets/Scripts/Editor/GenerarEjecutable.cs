using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;

public class GenerarEjecutable : Editor {

    [MenuItem("Generar/Ejecutable de Windowsx64")]
    public static void EjecutableWindowsX64()
    {
        Debug.LogWarning("--Generando Ejecutable de ROk--");
        BuildPlayerOptions BPO = new BuildPlayerOptions();
        BPO.scenes = new string[1] { @"Assets\Escenas\Terreno_Juego.unity" };
        BPO.options = BuildOptions.None;
        string Direccion = @"C:\Users\" + System.Environment.UserName + @"\Desktop\";
        Directory.CreateDirectory(Direccion + "ROK Build");
        Direccion += @"ROK Build\";
        BPO.locationPathName = Direccion + @"\ROK";
        BPO.target = BuildTarget.StandaloneWindows64;
        ////Serializacion

        //ManejadorGeneralMundo MGM = GameObject.FindGameObjectWithTag("MGM").GetComponent<ManejadorGeneralMundo>();

        //Stream stream;
        //Stream stream2;

        //BinaryFormatter Formateador = new BinaryFormatter();
        //stream = new FileStream(Direccion + @"\DatosN" + ".info", FileMode.Create, FileAccess.Write, FileShare.None);
        //Formateador.Serialize(stream, MGM.NombresDisponiblesSoldados);
        //stream.Close();

        //int contador2 = 0;
        //foreach (KeyValuePair<string, Sprite> CValor in MGM.RazasDisponibles)
        //{
        //    stream = new FileStream(Direccion + @"\DatosR" + contador2 + CValor.Key.GetType().ToString()
        //        + contador2 + ".info", FileMode.Create, FileAccess.Write, FileShare.None);
        //    stream2 = new FileStream(Direccion + @"\DatosR2" + contador2 + CValor.Value.GetType().ToString()
        //        + contador2 + ".info", FileMode.Create, FileAccess.Write, FileShare.None);
        //    Formateador.Serialize(stream, CValor.Key);
        //    Formateador.Serialize(stream2, CValor.Value.name);
        //    contador2++;
        //    stream.Close();
        //    stream2.Close();
        //}


        //int contador3 = 0;
        //foreach (KeyValuePair<string, __informacionPasivas> CValor in MGM.PasivasExistentes)
        //{
        //    stream = new FileStream(Direccion + @"\DatosP" + contador3 + CValor.Key.GetType().ToString()
        //        + contador3 + ".info", FileMode.Create, FileAccess.Write, FileShare.None);
        //    stream2 = new FileStream(Direccion + @"\DatosP2" + contador3 + CValor.Value.GetType().ToString()
        //        + contador3 + ".info", FileMode.Create, FileAccess.Write, FileShare.None);
        //    Formateador.Serialize(stream, CValor.Key);
        //    Formateador.Serialize(stream2, CValor.Value);
        //    contador3++;
        //    stream.Close();
        //    stream2.Close();
        //}


        //int contador4 = 0;
        //foreach (KeyValuePair<string, __informacionEnemigos> CValor in MGM.EnemigosExistentes)
        //{
        //    stream = new FileStream(Direccion + @"\DatosE" + contador4 + CValor.Key.GetType().ToString()
        //        + contador4 + ".info", FileMode.Create, FileAccess.Write, FileShare.None);
        //    stream2 = new FileStream(Direccion + @"\DatosE2" + contador4 + CValor.Value.GetType().ToString()
        //        + contador4 + ".info", FileMode.Create, FileAccess.Write, FileShare.None);
        //    Formateador.Serialize(stream, CValor.Key);
        //    Formateador.Serialize(stream2, CValor.Value);
        //    contador4++;
        //    stream.Close();
        //    stream2.Close();
        //}



        string res = BuildPipeline.BuildPlayer(BPO);
        string[] DirArchivosExcel = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Informacion en Excel\");
        foreach (string CDirExcel in DirArchivosExcel)
        {
            if (!CDirExcel.Contains(".meta"))
            {
                string nombre = new FileInfo(CDirExcel).Name;
                try
                {
                    File.Copy(CDirExcel, Direccion + @"\" + nombre);
                }
                catch (Exception e)
                {
                    Debug.Log("Error. Mensaje: " + e.Message);
                }
            }
        }
        File.Move(Direccion + "ROK.", Direccion + "ROK.exe");
        Debug.LogWarning("--Resultado: " + res + "--");
    }
	
}
