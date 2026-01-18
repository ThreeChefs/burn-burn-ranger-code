public class MaterialItemSlot : ItemSlot
{
    public ComposeItemSlot Target { get; private set; }

    protected override void OnClickButton()
    {
        ResetSlot();
    }

    public void SetSlot(ItemInstance itemInstance, ComposeItemSlot target)
    {
        SetSlot(itemInstance);
        target.IsMaterial = true;
        Target = target;
    }

    public override void ResetSlot()
    {
        base.ResetSlot();
        Target.IsMaterial = false;
        Target = null;
    }
}
