using UnityEngine;
public abstract class GenericAction : IAction
{
    public ButtonContext requiredPull, requiredPush;
    public bool canInterrupt;
    public float cooldown, timeLeft;
    public GenericAction(ButtonContext requiredPull, ButtonContext requiredPush, bool canInterrupt, float cooldown)
    {
        this.requiredPull = requiredPull;
        this.requiredPush = requiredPush;
        this.canInterrupt = canInterrupt;
        this.cooldown = cooldown;
        state = ActionState.Pending;
        lastState = ActionState.Dead;
    }
    public abstract short Complexity { get; }
    public ActionState State { get => state; }
    public ActionState state;
    private ActionState lastState;

    public virtual bool CanFire(ActionContext context)
    {
        return context.push == requiredPush && context.pull == requiredPull && (canInterrupt || context.currentAction == null || context.currentAction.State==ActionState.Dead);
    }
    public virtual void Fire(ActionContext context)
    {
        if (context.currentAction != null && context.currentAction.State != ActionState.Dead)
        {
            context.currentAction.Abort();
        }
        context.currentAction = this;
        state = ActionState.Running;
    }
    public virtual bool FixedUpdate(ActionContext context)
    {
        if (state == ActionState.Stale)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                state = ActionState.Dead;
            }
        }

        if (lastState!=state)
        {
            if (state == ActionState.Stale)
            {
                timeLeft = cooldown;
            }

            lastState = state;
            return true;
        }
        return false;

    }
    public virtual void Abort()
    {
        state = ActionState.Dead;
    }
}