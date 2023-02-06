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

    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing;
    private float dashPower = 70f;
    private float dashTime = 0.2f;
    private float dashCounter = 1f;

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
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            StartCoroutine(Dash());

        Flip();
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

        if(_moveInput.x > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if(_moveInput.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    void Flip()
    {

    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        // tira a gravidade do player durante o dash
        float originalGravity = rig.gravityScale;
        rig.gravityScale = 0f;
        rig.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        rig.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCounter);
        canDash = true;
    }
}
