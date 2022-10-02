using UnityEngine;

public abstract class GenericShove : GenericAction
{
    public float speed, force, lifetime, recoil;
    public IInteractable target;
    public ShoveInstance shoveInstance;

    public GenericShove(float cooldown, float lifetime, float speed, float force, float recoil) : base(ButtonContext.Released, ButtonContext.Pressing, false, cooldown)
    {
        this.speed = speed;
        this.force = force;
        this.lifetime = lifetime;
        this.recoil = recoil;
    }

    public override bool CanFire(ActionContext context)
    {
        return base.CanFire(context);
    }
    public override void Fire(ActionContext context)
    {
        base.Fire(context);
        target = null;
        shoveInstance = GameObject.Instantiate(ShoveInstance.Prefab).GetComponent<ShoveInstance>();
        Vector2 playerPos = Player.instance.transform.position;
        shoveInstance.Initialize(this, playerPos, PlayerMouse.pos - playerPos, speed, lifetime);
    }
    public virtual void Hit(IInteractable target)
    {
        this.target = target;
        Vector2 direction = (target.Position() - Player.instance.Position()).normalized;
        target.ApplyForce(direction * force);
        Player.instance.ApplyForce(direction * recoil * -1);
        //EndShove();
    }

    public void EndShove()
    {
        if (state != ActionState.Running)
        {
           return;
        }
        GameObject.Destroy(shoveInstance.gameObject);
        state = ActionState.Stale;
    }

    public override void Abort() {
        EndShove();
        base.Abort();
    }
}

