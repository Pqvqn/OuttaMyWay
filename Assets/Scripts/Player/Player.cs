using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CircleCollider2D collider;
    private static readonly float speed = 5f;
    public InputAction playerControls;
    Vector2 moveDirection = Vector2.zero;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    void Start()
    { 
    }

    void Update()
    {
        moveDirection = Quaternion.Euler(0, 0, -45) * -playerControls.ReadValue<Vector2>();
    }

    Vector2 velocity = Vector2.zero;
    void FixedUpdate()
    {
        Collider2D[] around = Physics2D.OverlapCircleAll(transform.position, collider.radius, 1 << 7);
        velocity = Vector2.Lerp(velocity, moveDirection, 3 * Time.deltaTime);
        foreach (Collider2D other in around)
        {
            Civilian c = other.GetComponent<Civilian>();
            if (c!=null)
            {
                velocity += c.Collision(transform.position, velocity, 1f);
                transform.position = (Vector2)other.transform.position + (collider.radius + Civilian.radius) * ((Vector2)(transform.position - other.transform.position)).normalized;
            }
        }
        transform.position += (Vector3) velocity * speed * Time.deltaTime;
    }
}
