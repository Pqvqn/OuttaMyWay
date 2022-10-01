using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CircleCollider2D collider;
    private static readonly float speed = 0.5f;
    public InputAction playerControls;
    Vector3 moveDirection = Vector3.zero;

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
        //moveDirection = Camera.main.transform.TransformDirection(playerControls.ReadValue<Vector2>()).normalized;
        moveDirection = Quaternion.Euler(0, 0, -45) * -playerControls.ReadValue<Vector2>();
        Debug.Log(moveDirection);
    }

    void FixedUpdate()
    {
        transform.position +=  moveDirection * speed;
    }
}
