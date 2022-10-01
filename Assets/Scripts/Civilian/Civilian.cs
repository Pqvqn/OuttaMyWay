using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour
{
    private CircleCollider2D collider;
    private static readonly float speed = 5;
    private IRoom currentRoom;

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
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

    private float patience = 0;
    private Vector2 target = Vector2.zero, velocity = Vector2.zero;
    void FixedUpdate()
    {
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
        Collider2D[] around = Physics2D.OverlapCircleAll((Vector2)transform.position, 3f, 1);
        if (around.Length > 1)
        {
            Vector2 gravity = Vector2.zero;
            foreach (Collider2D other in around)
            {
                if (other == collider) continue;
                Civilian c = other.GetComponent<Civilian>();
                if (c != null)
                {
                    Vector2 dir = (Vector2)transform.position - (Vector2)c.transform.position;
                    float sqrMag = dir.SqrMagnitude();
                    if (sqrMag > 0.1f)
                    {
                        sqrMag *= sqrMag;
                        gravity += 3 * dir / sqrMag;
                    }
                }
            }
            velocity = Vector2.Lerp(velocity, gravity, Time.deltaTime);
        }
        transform.position += (Vector3)velocity * Time.deltaTime * speed;
    }
}
