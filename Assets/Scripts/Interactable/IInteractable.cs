using UnityEngine;

public interface IInteractable
{
    public bool KnockDown();
    public bool ApplyForce(Vector2 force);
    public IInteractable Holder { get; set; }
    public Vector2 HoldDirection();
    public Vector2 Position();
}