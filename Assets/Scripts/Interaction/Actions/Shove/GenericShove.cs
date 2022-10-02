using UnityEngine;

public abstract class GenericShove : GenericAction
{
    public float lastUse, cooldown, speed;
    public GameObject target;
    public ShoveInstance shoveInstance;
    public override float Stale { get { return stale; } }
    private float stale = 0;
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
        this.stale = -1;
        shoveInstance = GameObject.Instantiate(ShoveInstance.Prefab).GetComponent<ShoveInstance>();
        Vector2 playerPos = Player.instance.transform.position;
        shoveInstance.Initialize(this, playerPos, PlayerMouse.pos - playerPos, speed);
    }
    public virtual void Hit(GameObject target)
    {
        this.target = target;
        Debug.Log(target);
        this.stale = Time.time;
    }
    public override void FixedUpdate(ActionContext context) {}
    public override void Abort() {
        GameObject.Destroy(shoveInstance.gameObject);
        this.stale = Time.time;
    }
}

