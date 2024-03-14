using System.IO;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Json�� �Ľ� �Ͽ� �ҷ����� �Լ� �Դϴ�
    /// </summary>
    /// <typeparam name="TLoad">������ ���̽� �̸�</typeparam>
    /// <param name="filename"></param>
    /// <returns>�Ľ̵� ������ ���̽�</returns>
    public static TLoad LoadJson<TLoad>(string filename)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Json/{filename}");
        if (jsonFile != null)
        {
            string json = jsonFile.text;
            return JsonUtility.FromJson<TLoad>(json);
        }
        Debug.Log($"There is no file {filename}");
        return default;
    }
    public static void SaveToJson<TSave>(TSave data, string jsonDataPath)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(jsonDataPath, json);
    }
    public static void DeleteJson(string jsonDataPath)
    {
        File.Delete(jsonDataPath);
    }
}
