using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string SceneName;
    public Vector3 PlayerPosition;
    public int ClearChapterNumber;
    public string PlayerPositionX;
    public string PlayerPositionY;
    public string PlayerPositionZ;

    public bool hasWatchedIntroVideo;
}
