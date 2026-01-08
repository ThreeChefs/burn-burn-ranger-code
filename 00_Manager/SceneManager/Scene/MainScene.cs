public class MainScene : BaseScene
{
    private void Start()
    {
        UIManager.Instance?.SpawnUI(UIName.UI_StageSelect);
        UIManager.Instance.LoadUI(UIName.UI_Equipment);
        UIManager.Instance.LoadUI(UIName.UI_Shop);
        UIManager.Instance.SpawnUI(UIName.UI_Home);
    }
}
