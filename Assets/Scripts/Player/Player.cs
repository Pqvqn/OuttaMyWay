using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CircleCollider2D collider;
    private static readonly float speed = 2;
    public InputAction playerControls;
    Vector2 moveDirection = Vector2.zero;

    private void onEnable()
    {
        playerControls.Enable();
    }

    private void onDisable()
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
        moveDirection = playerControls.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        
    }
}
