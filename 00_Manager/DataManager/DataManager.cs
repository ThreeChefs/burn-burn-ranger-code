using System.IO;
using UnityEngine;

public class DataManager
{
    private SaveData saveData = new SaveData();


    private readonly string savePath;

    public DataManager()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void Save(PlayerCondition condition)
    {
        saveData.playerProgress = condition.ExportProgress();
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"[DataManager] Save OK: {savePath}\n{json}");
    }

    public void Load(PlayerCondition condition)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning($"파일이 없어요~");
            return;
        }
        string json = File.ReadAllText(savePath);
        var loaded = JsonUtility.FromJson<SaveData>(json);

        condition.ImportProgress(loaded.playerProgress);
        saveData = loaded;

    }

    public void ReStart(PlayerCondition condition, bool deleteSaveFile = true)
    {
        saveData = new SaveData();
        condition.GlobalLevel.SetLevelAndExp(1, 0);

        if (deleteSaveFile && File.Exists(savePath))
            File.Delete(savePath);

        Debug.Log($"[DataManager] ResetForTest 완료 (deleteSaveFile={deleteSaveFile})");
    }
}
