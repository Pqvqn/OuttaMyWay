using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour
{
    private CircleCollider2D collider;
    private static readonly float speed = 2;

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
    }

    public void Avoid(Civilian other)
    {
        velocity = ((Vector2)Vector3.Cross(other.transform.position - transform.position, Vector3.forward)).normalized;
    }

    private float patience = 0;
    private Vector2 target = Vector2.zero, velocity = Vector2.zero;
    void FixedUpdate()
    {
        patience -= Time.deltaTime;
        if (patience <= 0)
        {
            target = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            patience = Random.Range(2f, 7f);
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, 0.7f, 1);
        if (colliders.Length > 1)
        {
            if (colliders.Length == 2)
            {
                Collider2D other = colliders[0] != collider ? colliders[0] : colliders[1];
                Civilian c = other.GetComponent<Civilian>();
                if (c != null)
                {
                    c.Avoid(this);
                    Avoid(c);
                }
                transform.position += (Vector3)velocity * Time.deltaTime * speed;
            }
            else
            {
                Vector2 move = Vector2.zero;
                foreach (Collider2D collider in colliders)
                {
                    Civilian c = collider.GetComponent<Civilian>();
                    if (c != null)
                    {
                        move += (Vector2)transform.position - (Vector2)c.transform.position;
                    }
                    move.Normalize();
                    transform.position += (Vector3)move * Time.deltaTime * speed;
                }
            }
        } else
        {
            velocity = Vector2.Lerp(velocity, (target - (Vector2)transform.position).normalized, Time.deltaTime);
            transform.position += (Vector3)velocity * Time.deltaTime * speed;
        }
    }
}
