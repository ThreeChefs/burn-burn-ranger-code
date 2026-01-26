using UnityEngine;

public class DimmedUI : BaseUI
{

    public void SetSiblingOrder(Transform target)
    {
        if (target == null) return;

        Transform targetParent = target.parent;
        if (targetParent == null) return;

        transform.SetParent(targetParent, false);
        transform.SetSiblingIndex(target.GetSiblingIndex() + 1);
    }


}
