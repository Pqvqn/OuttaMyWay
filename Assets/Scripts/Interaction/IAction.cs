public interface IAction
{
    public bool CanFire(ActionContext context);
    public void Fire(ActionContext context);
    public void FixedUpdate(ActionContext context);
    public void Abort();
    public short Complexity { get; }
    public float Stale { get; }
}