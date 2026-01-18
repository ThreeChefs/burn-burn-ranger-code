using System;
using UnityEngine;

public class ComposeItemSlot : ItemSlot
{
    [SerializeField] private GameObject _lock;

    public event Action<ComposeItemSlot, ItemInstance> OnClickSlot;

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
        OnClickSlot?.Invoke(this, instance);
    }

    public bool EqualsItemClassAndData(ItemInstance itemInstance)
    {
        return instance.ItemData.Id == itemInstance.ItemData.Id
            && instance.ItemClass == itemInstance.ItemClass;
    }

    #region 버튼
    public void LockButton()
    {
        button.enabled = false;
        _lock.SetActive(true);
    }

    public void UnLockButton()
    {
        button.enabled = true;
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
