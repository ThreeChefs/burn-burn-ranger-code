public class MainScene : BaseScene
{
    private void Start()
    {
        UIManager.Instance.LoadUI(UIName.UI_HomeBottomMenu);
        UIManager.Instance.LoadUI(UIName.UI_HomeTopBar);
        
        UIManager.Instance.LoadUI(UIName.UI_Home);

        UIManager.Instance.LoadUI(UIName.UI_Equipment, false);
        UIManager.Instance.LoadUI(UIName.UI_ItemDetail, false);
        UIManager.Instance.LoadUI(UIName.UI_ItemCompose, false);

        UIManager.Instance.LoadUI(UIName.UI_Shop, false);
        UIManager.Instance.LoadUI(UIName.UI_PickUp, false);

        UIManager.Instance.LoadUI(UIName.UI_Growth, false);

        UIManager.Instance.LoadUI(UIName.UI_InGameLoading, false);

        SoundManager.Instance.PlayBgm(BgmName.Main, volume: 0.6f);
    }

    private void OnDestroy()
    {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.StopAllSfx();
    }
}
