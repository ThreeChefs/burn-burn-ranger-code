using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneBoxSlot : BaseSlot
{
    [SerializeField] GameObject _focus;
 

    public void SetFocus(bool focus)
    {
        _focus.gameObject.SetActive(focus);
    }
}
