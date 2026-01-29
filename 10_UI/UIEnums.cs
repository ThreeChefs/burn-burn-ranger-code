public enum UIName
{
    // In Stage
    UI_Stage,
    UI_SkillSelect,
    UI_StageProgressBar,
    UI_Victory,
    UI_Defeat,
    UI_StagePause,
    UI_WarnningSign,
    UI_JoyStick,
    UI_FortuneBox,
    UI_SkillDamageData,

    // Outside of Stage
    UI_Home,
    UI_HomeBottomMenu,
    UI_HomeTopBar,

    UI_Settings,

    UI_Shop,
    UI_Equipment,
    UI_ItemDetail,
    UI_ItemCompose,
    UI_PickUp,
    UI_Growth,

    UI_StageSelect,

    // Loading UI
    UI_BootstrapLoading,
    UI_InGameLoading,

    // World UI
    WorldUI_Hp,

    // Common
    UI_Dimmed
}

public enum UICanvasOrder
{
    Background = 0,
    Default = 100,
    Popup = 200,
    TopMost = 300,
    PopupTopMost = 400
}

public enum BottomBarMenuType
{
    Home,
    Growth,
    Equipment,
    Shop,
    Challenge,

}