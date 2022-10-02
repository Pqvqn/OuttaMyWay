public abstract class GenericAction : IAction
{
    public ButtonContext requiredPull, requiredPush;
    public bool canInterrupt;
    public GenericAction(ButtonContext requiredPull, ButtonContext requiredPush, bool canInterrupt)
    {
        this.requiredPull = requiredPull;
        this.requiredPush = requiredPush;
        this.canInterrupt = canInterrupt;
    }
    public abstract short Complexity { get; }
    public abstract float Stale { get; }

    public virtual bool CanFire(ActionContext context)
    {
        return context.push == requiredPush && context.pull == requiredPull && (canInterrupt || context.currentAction.Stale != -1);
    }
    public virtual void Fire(ActionContext context)
    {
        context.currentAction = this;
    }
    public abstract void FixedUpdate(ActionContext context);
    public abstract void Abort();
}