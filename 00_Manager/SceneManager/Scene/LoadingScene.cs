/// <summary>
/// 로딩씬
/// </summary>
public class LoadingScene : BaseScene
{
    private void Start()
    {
        UIManager.Instance.LoadUI(UIName.UI_BootstrapLoading);
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.MainScene);
    }
}
