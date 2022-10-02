using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour, IInteractable
{
    [SerializeField] MeshRenderer hands;
    private Animation animation;
    private CircleCollider2D collider;
    private static readonly float speed = 5;
    private static readonly float mass = 1;
    public static readonly float radius = 0.5f;
    private IRoom currentRoom;

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        animation = GetComponent<Animation>();
    }

    void Start()
    {
    }

    public void SetRoom(IRoom newRoom)
    {
        currentRoom = newRoom;
    }
    public void Avoid(Civilian other, float alpha)
    {
        velocity = Vector2.Lerp(velocity, ((Vector2)Vector3.Cross(other.transform.position - transform.position, Vector3.forward)) * 0.2f, alpha);
    }

    public Vector2 Collision(Vector2 otherPosition, Vector2 otherVelocity, float otherMass)
    {
        Vector2 diff = (otherPosition - (Vector2) transform.position).normalized;
        float dot = Vector2.Dot(diff, velocity), dotOther = Vector2.Dot(diff, otherVelocity);
        if (dot < dotOther)
        {
            return Vector2.zero;
        }
        Vector2 diffProj = dot * diff;
        Vector2 diffProjOther = dotOther * diff;
        Vector2 finalDiffVelocity = Vector2.Lerp(diffProjOther, diffProj, mass / (mass + otherMass));
        velocity = velocity - diffProj + finalDiffVelocity;
        return finalDiffVelocity - diffProjOther;
    }

    private float patience = 0;
    private Vector2 target = Vector2.zero, velocity = Vector2.zero, knockback = Vector2.zero;

    void FixedUpdate()
    {
        hands.enabled = Holder != null;

        patience -= Time.deltaTime;
        if (patience <= 0)
        {
            patience = Random.Range(5f, 8f);
            if (currentRoom != null)
            {
                target = currentRoom.RandomWanderPosition(transform.position);
            } else
            {
                target = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));
            }
        }

        Vector2 clamp;
        if (currentRoom.NeedsClamp(transform.position, out clamp))
        {
            if (currentRoom.Prev != null && currentRoom.Prev.Within(transform.position))
            {
                currentRoom = currentRoom.Prev;
                patience = 0;
            } else if (currentRoom.Next != null && currentRoom.Next.Within(transform.position))
            {
                currentRoom = currentRoom.Next;
                patience = 0;
            } else
            {
                transform.position = clamp;
            }
        }

        velocity = Vector2.Lerp(velocity, (target - (Vector2)transform.position).normalized, Time.deltaTime);

        if(knockback.magnitude > .1)
        {
            velocity = knockback;
            knockback *= 1 - Time.deltaTime*50;
        }

        Collider2D[] around = Physics2D.OverlapCircleAll((Vector2)transform.position, 3f, 1 << 7 | 1 << 8);
        if (around.Length > 1)
        {
            Vector2 repulsion = Vector2.zero;
            foreach (Collider2D other in around)
            {
                if (other == collider) continue;
                Repulsion r = other.GetComponent<Repulsion>();
                if (r != null)
                {
                    repulsion += r.CalcRepulsion(transform.position);
                }
            }
            velocity = Vector2.Lerp(velocity, repulsion, Time.deltaTime);
        }

        if (velocity.magnitude > 0.3f)
        {
            transform.localScale = new Vector3(velocity.x > 0 ? -1 : 1, 1, 1);
            animation.wrapMode = WrapMode.Loop;
            animation.Play();
        }
        else
        {
            animation.wrapMode = WrapMode.Clamp;
        }
        transform.position += (Vector3)velocity * Time.deltaTime * speed;

        if (Holder == null)
        {
            transform.position += (Vector3)velocity * Time.deltaTime * speed;
        }
        else
        {
            transform.position = Player.instance.Position() + Player.instance.HoldDirection() * 1.2f;
        }

    }

    public IInteractable Holder { get; set; }
    public IInteractable Holdee { get; set; }
    public void Hold(IInteractable target, bool grab)
    {
        if (grab)
        {
            target.Holder = this;
            Holdee = target;
        }
        else
        {
            target.Holder = null;
            Holdee = null;
        }
    }
    public bool KnockDown()
    {
        throw new System.NotImplementedException();
    }
    public bool ApplyForce(Vector2 force)
    {
        knockback = force/mass;
        return true;
    }
    public Vector2 HoldDirection()
    {
        throw new System.NotImplementedException();
    }

    public Vector2 Position()
    {
        return transform.position;
    }

}
