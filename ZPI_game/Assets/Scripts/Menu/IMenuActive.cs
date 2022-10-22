internal interface IMenuActive
{
    public void SetEnabled(bool isActive);
    public bool IsEnabled { get; }
}