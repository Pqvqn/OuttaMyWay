using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IInteractable
{
    private Animation animation;
    private Rigidbody2D rb;
    private CircleCollider2D collider;
    public InputAction playerControls;
    Vector2 moveDirection = Vector2.zero; //, knockback = Vector2.zero;
    [SerializeField] float acceleration = 7f;
    [SerializeField] float speed = 10f;
    [SerializeField] float mass = 0.5f;
    [SerializeField] Transform flipper;
    [SerializeField] MeshRenderer hands;

    public void ShowHands()
    {
        hands.enabled = true;
    }
    public void HideHands()
    {
        hands.enabled = false;
    }

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
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        animation = GetComponent<Animation>();
        qTransform = Quaternion.AngleAxis(Vector3.Angle(Vector3.ProjectOnPlane(UnityEngine.Camera.main.transform.forward, Vector3.forward), Vector3.up), Vector3.forward);
    }
    void Start()
    {
        ActionKnowledge.Clear();
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
                Vector2 dir = other.transform.position - transform.position;
                if (dir.magnitude < (collider.radius + Civilian.radius)) {
                    other.transform.position = (Vector2)transform.position + (collider.radius + Civilian.radius) * dir.normalized;
                }
            }
        }
        if (velocity.magnitude > 0.1f)
        {
            flipper.localScale = new Vector3(velocity.x > 0 ? -1 : 1, 1, 1);
            animation.wrapMode = WrapMode.Loop;
            animation.Play();
        }
        else
        {
            animation.wrapMode = WrapMode.Clamp;
        }
        rb.velocity = (Vector3) velocity * speed;
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
            Player.instance.ShowHands();
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
        velocity = force / mass;
        return true;
    }
    public Vector2 HoldDirection()
    {
        return (PlayerMouse.pos - Position()).normalized;
    }

    public Vector2 Position()
    {
        return rb.position;
    }

}
