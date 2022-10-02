using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Release : GenericAction
{
    public IInteractable target;
    public GrabInstance grabInstance;
    public override short Complexity { get => 2; }
    public Release() : base(ButtonContext.Released, ButtonContext.Released, false, 0f)
    {
        
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
        state = ActionState.Stale;
    }

    public override void Abort()
    {
        base.Abort();
    }
}
