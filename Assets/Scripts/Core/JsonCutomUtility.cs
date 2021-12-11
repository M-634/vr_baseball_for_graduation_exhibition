using UnityEngine;
using System.IO;
using System.Runtime.Serialization;


public static class JsonCutomUtility
{
    public interface ISerializeToJson { }
    public static void SaveDataToJson<T>(string path, T data) where T : ISerializeToJson
    {
        var json = JsonUtility.ToJson(data, true);

        try
        {
            using var writer = new StreamWriter(path, false);
            writer.Write(json);
            Debug.Log($"Saving {data}...");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    public static bool LoadDataFromJson<T>(string path, ref T data) where T : ISerializeToJson
    {
        try
        {
            using var reader = new StreamReader(path);
            var json = reader.ReadToEnd();
            data = JsonUtility.FromJson<T>(json);

            Debug.Log($"Loading {data}...");
            return true;

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
            return false;
        }
    }
}
