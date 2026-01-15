using System;

[Serializable]
public class SaveData
{
    public PlayerProgressSave playerProgress = new PlayerProgressSave();
}

[Serializable]
public class PlayerProgressSave
{
    public int level = 1;
    public float currentExp = 0f;
}
