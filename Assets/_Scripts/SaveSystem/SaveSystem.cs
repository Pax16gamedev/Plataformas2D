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
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        File.WriteAllText(path, json);
        print($"Datos guardados en: {path} en la fecha {data.lastSaveDate}");
    }

    public static GameData LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);
            print("Datos cargados correctamente.");
            return data;
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de guardado.");
            return new GameData(); // Retorna datos por defecto.
        }
    }

}
