using System;
using UnityEngine;

public class ComposeItemSlot : ItemSlot
{
    [SerializeField] private GameObject _lock;
    public event Action<ItemInstance> OnClickSlot;

    protected override void OnEnable()
    {
        base.OnEnable();
        UnLockButton();
    }

    private void OnDestroy()
    {
        OnClickSlot = null;
    }

    protected override void OnClickButton()
    {
        OnClickSlot?.Invoke(instance);
    }

    #region 버튼
    public void LockButton()
    {
        button.gameObject.SetActive(false);
        _lock.SetActive(true);
    }

    public void UnLockButton()
    {
        button.gameObject.SetActive(true);
        _lock.SetActive(false);
    }
    #endregion

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        _lock = transform.FindChild<Transform>("Lock").gameObject;
    }
#endif
}
