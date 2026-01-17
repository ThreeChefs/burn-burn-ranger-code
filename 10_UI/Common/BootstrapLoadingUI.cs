using UnityEngine;
using UnityEngine.UI;

public class BootstrapLoadingUI : BaseUI
{
    [SerializeField] private Image _progress;

    public void SetProgress(float progress)
    {
        _progress.fillAmount = progress;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _progress = transform.FindChild<Image>("Image - Progress");
    }
#endif
}
