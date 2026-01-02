public class GameManager : GlobalSingletonManager<GameManager>
{
    // 각종 매니저들
    public SceneController Scene = new();
    public DataManager Data = new();

    private void OnApplicationQuit()
    {
        Data.Save();
    }
}