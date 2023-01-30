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

    [Header("Jump")]
    public float jumpForce;
    private float jumpBufferLength = .1f;
    private float jumpBufferCounter;
    private float coyoteTime = .2f;
    private float coyoteCounter;

    private bool isGrounded;

    [Space(15)]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .1f, groundLayer);
    }

    void FixedUpdate()
    {
        Move();
        #region Jump
        if(Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferLength;
        else
            jumpBufferCounter -= Time.deltaTime;

        if(isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        if(jumpBufferCounter >= 0 && coyoteCounter > 0f)
            rig.velocity = new Vector2(rig.velocity.x, jumpForce);
            jumpBufferCounter = 0;

        if(Input.GetButtonUp("Jump") && rig.velocity.y > 0)
            rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * .5f);
        #endregion
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
