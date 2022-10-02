using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : GenericAction
{
    public IInteractable target;
    public GrabInstance grabInstance;
    private float force;
    public override short Complexity { get => 3; }
    public Throw() : base(ButtonContext.Pressed, ButtonContext.Pressing, true, 0.25f)
    {
        this.force = 4f;
    }
    public override bool CanFire(ActionContext context)
    {
        return base.CanFire(context) && Player.instance.Holdee != null;
    }
    public override void Fire(ActionContext context)
    {
        base.Fire(context);
        target = Player.instance.Holdee;
        Player.instance.Hold(target, false);
        Vector2 direction = (target.Position() - Player.instance.Position()).normalized;
        target.ApplyForce(direction * force);
        state = ActionState.Stale;
    }

    public override void Abort()
    {
        base.Abort();
    }
}
