using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CircleCollider2D collider;
    public InputAction playerControls;
    Vector2 moveDirection = Vector2.zero;
    [SerializeField] float acceleration = 7f;
    [SerializeField] float speed = 10f;
    [SerializeField] float mass = 0.5f;


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public static Player instance;
    Quaternion qTransform = new Quaternion();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
        collider = GetComponent<CircleCollider2D>();
        qTransform = Quaternion.AngleAxis(Vector3.Angle(Vector3.ProjectOnPlane(UnityEngine.Camera.main.transform.forward, Vector3.forward), Vector3.up), Vector3.forward);
    }
    void Start()
    {
    }

    void Update()
    {
        moveDirection = qTransform * playerControls.ReadValue<Vector2>();
    }

    Vector2 velocity = Vector2.zero;
    void FixedUpdate()
    {
        Collider2D[] around = Physics2D.OverlapCircleAll(transform.position, collider.radius, 1 << 7);
        velocity = Vector2.Lerp(velocity, moveDirection, acceleration * Time.deltaTime);
        foreach (Collider2D other in around)
        {
            Civilian c = other.GetComponent<Civilian>();
            if (c!=null)
            {
                velocity += c.Collision(transform.position, velocity, mass);
                transform.position = (Vector2)other.transform.position + (collider.radius + Civilian.radius) * ((Vector2)(transform.position - other.transform.position)).normalized;
            }
        }
        transform.position += (Vector3) velocity * speed * Time.deltaTime;
    }
}
