public class MaterialItemSlot : ItemSlot
{
    public ComposeItemSlot Target { get; private set; }

    protected override void OnClickButton()
    {
        ResetSlot();
    }

    public void SetSlot(ItemInstance itemInstance, ComposeItemSlot target)
    {
        ResetSlot();

        SetSlot(itemInstance);
        Target = target;
        Target.gameObject.SetActive(false);
    }

    public override void ResetSlot()
    {
        base.ResetSlot();

        if (Target != null)
        {
            Target.gameObject.SetActive(true);
            Target = null;
        }
    }
}
