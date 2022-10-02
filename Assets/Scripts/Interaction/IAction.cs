public interface IAction
{
    public bool CanFire(ActionContext context);
    public void Fire(ActionContext context);
    public bool FixedUpdate(ActionContext context);
    public void Abort();
    public short Complexity { get; }
    public ActionState State { get; }
}