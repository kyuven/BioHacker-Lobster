using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Run")]
    public float moveSpeed;
    public float accelaration;
    public float deceleration;
    public float velocityPower;
    private Vector2 _moveInput;

    [SerializeField] private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float targetSpeed = _moveInput.x * moveSpeed;
        float speedDiff = targetSpeed - rig.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > .01f) ? accelaration : deceleration;
        float movement = (Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPower) * Mathf.Sign(speedDiff));

        rig.AddForce(movement * Vector2.right);
    }
}
