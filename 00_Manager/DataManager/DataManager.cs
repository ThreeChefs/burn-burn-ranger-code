using Newtonsoft.Json;
using System.IO;
using UnityEngine;
public class DataManager
{
    private SaveData saveData = new SaveData();
    private readonly string savePath;

    private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        ObjectCreationHandling = ObjectCreationHandling.Replace
    };
    public DataManager()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void Save(PlayerCondition condition, PlayerManager player, StageProgress stage, GrowthProgress growth)
    {
        saveData.playerProgress = condition.ExportProgress();
        saveData.inventory = player.SaveInventoryData();
        saveData.stageProgress = stage.ExportProgress();
        saveData.growthProgress = growth.ExportGrowthProgress();
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        string json = JsonConvert.SerializeObject(saveData, JsonSettings);
        File.WriteAllText(savePath, json);
        Debug.Log($"[DataManager] Save OK: {savePath}\n{json}");
    }

    public void Load(PlayerCondition condition, PlayerManager player, StageProgress stage, GrowthProgress growth)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning($"파일이 없어요~");
            return;
        }
        try
        {
            string json = File.ReadAllText(savePath);
            var loaded = JsonConvert.DeserializeObject<SaveData>(json, JsonSettings) ?? new SaveData();
            condition.ImportProgress(loaded.playerProgress);
            player.LoadInventoryData(loaded.inventory ?? new InventorySaveData());
            stage.ImportStageProgress(loaded.stageProgress ?? new StageProgressSaveInfo());
            var gp = loaded.growthProgress ?? new GrowthProgressSaveInfo();
            growth.ImportGrowthPogress(gp);

            saveData = loaded;

            Debug.Log($"[DataManager] Load OK: {savePath}\n{json}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[DataManager] Load 실패: {ex}");
        }
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
