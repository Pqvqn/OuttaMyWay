using UnityEngine;
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
    public float Stale { get { return stale; } }
    private float stale = -1f;
    private bool lastStale = false;
    public void SetStale(bool stale)
    {
        if (stale)
        {
            this.stale = Time.time;
        } else
        {
            this.stale = -1f;
        }
    }

    public virtual bool CanFire(ActionContext context)
    {
        return context.push == requiredPush && context.pull == requiredPull && (canInterrupt || context.currentAction == null || context.currentAction.Stale < 0);
    }
    public virtual void Fire(ActionContext context)
    {
        context.currentAction = this;
    }
    public virtual bool FixedUpdate(ActionContext context)
    {
        if (lastStale!=(stale<0))
        {
            lastStale = stale<0;
            return true;
        }
        return false;
    }
    public abstract void Abort();
}