using UnityEngine;

public interface IInteractable
{
    public bool KnockDown();
    public bool ApplyForce(Vector2 force);
    public IInteractable Holder { get; set; }
    public IInteractable Holdee { get; }
    public void Hold(IInteractable target, bool grab);
    public Vector2 HoldDirection();
    public Vector2 Position();
}