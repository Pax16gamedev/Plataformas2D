using UnityEngine;
using System.IO;
using System;

public class SaveSystem : MonoBehaviour
{
    private const string SaveFileName = "GameData.json";

    public static void SaveGame(GameData data)
    {
        data.lastSaveDate = DateTime.UtcNow.ToLocalTime().ToString("o");
        string json = JsonUtility.ToJson(data, true);
        string path = GetSaveFilePath();
        File.WriteAllText(path, json); // No funciona para WebGL
    }

    public static GameData LoadGame()
    {
        string path = GetSaveFilePath();
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);
            print($"Datos cargados correctamente. Ultimo acceso {data.lastSaveDate}");
            return data;
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de guardado. Creando uno nuevo...");
            return new GameData(); // Retorna datos por defecto.
        }
    }

    private static string GetSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }

}
