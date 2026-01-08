public class MainScene : BaseScene
{
    private void Start()
    {
        UIManager.Instance?.SpawnUI(UIName.UI_StageSelect);
        UIManager.Instance.SpawnUI(UIName.UI_Home);
    }
}
