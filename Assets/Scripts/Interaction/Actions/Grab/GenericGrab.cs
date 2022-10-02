using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGrab : GenericAction
{
    public float speed, lifetime, recoil;
    public IInteractable target;
    public GrabInstance grabInstance;

    public override short Complexity { get; }

    public GenericGrab(float cooldown, float lifetime, float speed) : base(ButtonContext.Pressing, ButtonContext.Released, false, cooldown)
    {
        this.speed = speed;
        this.lifetime = lifetime;
    }

    public override bool CanFire(ActionContext context)
    {
        return base.CanFire(context);
    }
    public override void Fire(ActionContext context)
    {
        base.Fire(context);
        target = null;
        grabInstance = GameObject.Instantiate(GrabInstance.Prefab).GetComponent<GrabInstance>();
        Vector2 playerPos = Player.instance.transform.position;
        grabInstance.Initialize(this, playerPos, PlayerMouse.pos - playerPos, speed, lifetime);
    }
    public virtual void Hit(IInteractable target)
    {
        this.target = target;
        target.Holder = Player.instance;
    }

    public void EndGrab()
    {
        if (state != ActionState.Running)
        {
            return;
        }
        GameObject.Destroy(grabInstance.gameObject);
        state = ActionState.Stale;
        target.Holder = null;
    }

    public override void Abort()
    {
        base.Abort();
    }
}
