using UnityEngine;

public abstract class GenericShove : GenericAction
{
    public float lastUse, cooldown, speed;
    public IInteractable target;
    public ShoveInstance shoveInstance;

    public GenericShove(float cooldown, float speed) : base(ButtonContext.Released, ButtonContext.Pressing, false)
    {
        this.cooldown = cooldown;
        this.speed = speed;
        this.lastUse = 0;
    }

    public override bool CanFire(ActionContext context)
    {
        return base.CanFire(context) && lastUse < Time.time - cooldown;
    }
    public override void Fire(ActionContext context)
    {
        base.Fire(context);
        lastUse = Time.time;
        target = null;
        SetStale(false);
        shoveInstance = GameObject.Instantiate(ShoveInstance.Prefab).GetComponent<ShoveInstance>();
        Vector2 playerPos = Player.instance.transform.position;
        shoveInstance.Initialize(this, playerPos, PlayerMouse.pos - playerPos, speed);
    }
    public virtual void Hit(IInteractable target)
    {
        this.target = target;
        Debug.Log(target);
        SetStale(true);
    }
    public override void Abort() {
        GameObject.Destroy(shoveInstance.gameObject);
        SetStale(true);
    }
}

