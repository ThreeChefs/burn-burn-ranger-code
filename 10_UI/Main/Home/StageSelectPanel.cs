using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageSelectPanel : SwipeScrollPanel
{
    List<Image> _imgList = new List<Image>();

    protected List<Image> contentsImgList => _imgList;

    protected override void Awake()
    {
        base.Awake();

        _contents = new List<RectTransform>();

        for (int i = 0; i < GameManager.Instance.StageDatabase.Count; ++i)
        {
            RectTransform rect = Instantiate(_originContentPrefab, _contentsRect.transform);
            Image img = rect.GetComponent<Image>();

            if (img != null)
            {
                img.sprite = GameManager.Instance.StageDatabase[i].StageIcon;
                if (i <= GameManager.Instance.StageProgress.ClearStageNum)
                    img.material = null;
                _imgList.Add(img);
            }

            _contents.Add(rect);
        }

    }

}
