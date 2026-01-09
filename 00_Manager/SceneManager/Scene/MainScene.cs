public class MainScene : BaseScene
{
    private void Start()
    {
        UIManager.Instance?.SpawnUI(UIName.UI_StageSelect);
        UIManager.Instance.LoadUI(UIName.UI_Equipment, false);
        UIManager.Instance.LoadUI(UIName.UI_Shop, false);
        UIManager.Instance.LoadUI(UIName.UI_PickUp, false);
        UIManager.Instance.SpawnUI(UIName.UI_Home);
    }
}
