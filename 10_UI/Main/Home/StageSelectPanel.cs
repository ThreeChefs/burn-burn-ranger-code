using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageSelectPanel : SwipeScrollPanel
{
    private void Start()
    {
        _contents = new List<RectTransform>();

        for (int i = 0; i < GameManager.Instance.StageDatabase.Count; ++i)
        {
            RectTransform rect = Instantiate(_originContentPrefab, _contentsRect.transform);
            Image img = rect.GetComponent<Image>();

            if (img != null)
            {
                img.sprite = GameManager.Instance.StageDatabase[i].StageIcon;
            }
            
            _contents.Add(rect);
        }
    }
}
