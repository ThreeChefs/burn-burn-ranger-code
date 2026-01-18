using System;

[Serializable]
public class SaveData
{
    public PlayerProgressSave playerProgress = new PlayerProgressSave();

    public InventorySaveData inventory = new InventorySaveData();

    public StageProgressSaveInfo stageProgress = new StageProgressSaveInfo();
    public GrowthProgressSaveInfo growthProgress = new GrowthProgressSaveInfo();
}

[Serializable]
public class PlayerProgressSave
{
    public int level = 1;
    public float currentExp = 0f;
}
