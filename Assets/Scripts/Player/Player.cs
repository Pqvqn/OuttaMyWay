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
        Debug.Log(moveDirection);
    }

    Vector2 velocity = Vector2.zero, resistance = Vector2.zero;
    void FixedUpdate()
    {
        Collider2D[] around = Physics2D.OverlapCircleAll(transform.position, collider.radius, 1 << 7);
        velocity = Vector2.Lerp(velocity, moveDirection, 3 * Time.deltaTime);
        resistance = Vector2.Lerp(resistance, Vector2.zero, 0.2f * Time.deltaTime);
        foreach (Collider2D collider in around)
        {
            Civilian c = collider.GetComponent<Civilian>();
            if (c!=null)
            {
                resistance += c.Collision(transform.position, velocity + resistance, 0.2f);
                Debug.Log(resistance+","+velocity);
            }
        }
        transform.position += (Vector3) (velocity + resistance) * speed * Time.deltaTime;
    }
}
