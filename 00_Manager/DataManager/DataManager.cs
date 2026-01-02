using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager
{
    SaveData saveData;

    private const string FileName = "save.json";

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, FileName);    //저장경로
    public SaveData Current { get; private set; }

    public DataManager()
    {
        Current = new SaveData
        {
            SceneName = "",                 // 아직 미적용 (깡통상태)
            ClearChapterNumber = 0
        };
    }

    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            return;
        }
        string json = File.ReadAllText(SavePath);
        Current = JsonUtility.FromJson<SaveData>(json);

        if (Current == null)
        {
            Current = new SaveData();
        }
    }

    public void Save()
    {
        Logger.Log("데이터 저장");
        string json = JsonUtility.ToJson(Current, prettyPrint: true);
        File.WriteAllText(SavePath, json);
    }

    public void ClearSaveData()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
        Current = new SaveData(); // 메모리도 초기화
    }

    public void SavePlayingChapter(int currentChapterNumber)
    {
        Current.ClearChapterNumber = Mathf.Max(0, currentChapterNumber - 1);  // 현재 챕터가 2이면 현재 클리어한챕터 저장은 -1로  
        Save();
    }

    public void SavePlayerPosition(Transform player)
    {
        Vector3 pos = player.localPosition;
        Debug.Log($"[SAVE POS] {pos}");
        Current.PlayerPositionX = pos.x.ToString();    // string 형태로 position값 저장 (치환) 진우원튜터님 피드백
        Current.PlayerPositionY = pos.y.ToString();
        Current.PlayerPositionZ = pos.z.ToString();
        Debug.Log($"[SAVE STR] {Current.PlayerPositionX},{Current.PlayerPositionY},{Current.PlayerPositionZ}");
        Save();
    }

    public void LoadPlayerPosition(Transform player)
    {
        if (Current == null) return;

        float x = 0f;
        float y = 0f;
        float z = 0f;

        float.TryParse(Current.PlayerPositionX, out x);
        float.TryParse(Current.PlayerPositionY, out y);
        float.TryParse(Current.PlayerPositionZ, out z);

        Vector3 loaded = new Vector3(x, y, z);
        player.localPosition = loaded;
    }

    public void SaveSceneName()
    {
        Current.SceneName = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// [public] 인트로 비디오를 볼 경우 호출
    /// </summary>
    public void WatchIntroVideo()
    {
        Current.hasWatchedIntroVideo = true;
        Logger.Log("인트로 영상 보기 완료");
    }
}
