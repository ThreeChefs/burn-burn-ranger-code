/// <summary>
/// 로딩씬
/// </summary>
public class LoadingScene : BaseScene
{
    private void Start()
    {
        GameManager.Instance.Scene.LoadSceneWithCoroutine(SceneType.MainScene);
    }
}
