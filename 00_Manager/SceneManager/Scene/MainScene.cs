
public class MainScene : BaseScene
{
    private void Start()
    {
        BottomBarUI bottom = (BottomBarUI)UIManager.Instance.LoadUI(UIName.UI_HomeBottomMenu);
        TopBarUI top = (TopBarUI)UIManager.Instance.LoadUI(UIName.UI_HomeTopBar);

        if (top != null && bottom != null)
        {
            // 이벤트 연결
            // todo : 이벤트 버스 추가해보기
            bottom.OnClickMenuAction += top.ChangeBottomMenu;
        }

        UIManager.Instance.LoadUI(UIName.UI_Home);

        UIManager.Instance.LoadUI(UIName.UI_Settings, false);

        UIManager.Instance.LoadUI(UIName.UI_Equipment, false);
        UIManager.Instance.LoadUI(UIName.UI_ItemDetail, false);
        UIManager.Instance.LoadUI(UIName.UI_ItemCompose, false);

        UIManager.Instance.LoadUI(UIName.UI_Shop, false);
        UIManager.Instance.LoadUI(UIName.UI_PickUp, false);

        UIManager.Instance.LoadUI(UIName.UI_Growth, false);

        UIManager.Instance.LoadUI(UIName.UI_InGameLoading, false);

        SoundManager.Instance.PlayBgm(BgmName.Main, volume: 0.6f);

    }

    private void OnDisable()
    {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.StopAllSfx();
    }
}

