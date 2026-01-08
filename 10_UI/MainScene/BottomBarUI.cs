using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomBarUI : BaseUI
{
    [SerializeField] private float _originPosY = 0f;
    [SerializeField] private float _targetPosY = 50f;
    [SerializeField] private float _duration = 1f;

    [Header("버튼")]
    [SerializeField] private Button[] _buttons;
    [SerializeField] private RectTransform[] _imageTransforms;
    [SerializeField] private TextMeshProUGUI[] _texts;

    private int _index;

    private void OnEnable()
    {
        _index = -1;

        for (int i = 0; i < _buttons.Length; i++)
        {
            int index = i;
            _buttons[i].onClick.AddListener(() => { OnClickButton(index); });
        }
    }

    private void OnDisable()
    {
        foreach (Button button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    // todo: 누군가는 해야하는 버튼 클릭하면 나오는 애니메이션 
    private void OnClickButton(int index)
    {
        if (_index == index)
            return;

        if (_index != -1)
        {
            _imageTransforms[_index].DOKill();
            _imageTransforms[_index].DOAnchorPosY(_originPosY, _duration);
            _texts[_index].gameObject.SetActive(false);
        }

        _imageTransforms[index].DOKill();
        _imageTransforms[index].DOAnchorPosY(_targetPosY, _duration);
        _texts[index].gameObject.SetActive(true);

        _index = index;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        _buttons = GetComponentsInChildren<Button>();

        _imageTransforms = new RectTransform[_buttons.Length];
        _texts = new TextMeshProUGUI[_buttons.Length];

        for (int i = 0; i < _buttons.Length; i++)
        {
            Transform transform = _buttons[i].transform;
            _imageTransforms[i] = transform.FindChild<RectTransform>("Image - Icon");
            _texts[i] = transform.FindChild<TextMeshProUGUI>("Text (TMP)");
        }
    }
#endif
}
