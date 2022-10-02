using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Release : GenericAction
{
    public IInteractable target;
    public GrabInstance grabInstance;
    public override short Complexity { get; }
    public Release() : base(ButtonContext.Releasing, ButtonContext.Released, false, 0.5f)
    {
        
    }
    public override bool CanFire(ActionContext context)
    {
        return base.CanFire(context);
    }
    public override void Fire(ActionContext context)
    {
        base.Fire(context);
        /*target = null;
        grabInstance = GameObject.Instantiate(GrabInstance.Prefab).GetComponent<GrabInstance>();
        Vector2 playerPos = Player.instance.transform.position;
        grabInstance.Initialize(this, playerPos, PlayerMouse.pos - playerPos, speed, lifetime);*/
    }
    public virtual void Hit(IInteractable target)
    {
        /*this.target = target;
        target.Holder = Player.instance;*/
    }

    public override void Abort()
    {
        base.Abort();
    }
}
