using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 5f;
    public float jumpingPower = 8f;
    public bool canJump = true;
    private bool isFacingRight = true;

    public PhysicsMaterial2D bounceMat, normalMat;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    // Update is called once per frame
    void Update()
    {
       horizontal = Input.GetAxisRaw("Horizontal");

       if (jumpingPower > 0)
       {
        rb.sharedMaterial = bounceMat;
       }
       else
       {
        rb.sharedMaterial = normalMat;
       }


       if (Input.GetKey("space") && IsGrounded() && canJump)
       {
        jumpingPower += 0.2f;
       }

       if (Input.GetKeyDown("space") && IsGrounded() && canJump)
       {
        rb.velocity = new Vector2 (0.0f, rb.velocity.y);
       }

       if (jumpingPower >= 20 && IsGrounded())
       {
        float tempx = horizontal * speed;
        float tempy = jumpingPower;
        rb.velocity = new Vector2 (tempx, tempy);
        Invoke("ResetJump", 0.1f);
       }

       if (Input.GetKeyUp("space"))
       {
        if (IsGrounded())
        {
            rb.velocity = new Vector2 (horizontal * speed, jumpingPower);
            jumpingPower = 0.0f;
        }
        canJump = true;
       }

       
       Flip();
    
    }

    private void FixedUpdate()
    {
        if (jumpingPower == 0.0f && IsGrounded())
       {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
       }
    
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.35f,0.11f), CapsuleDirection2D.Horizontal,0, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void ResetJump()
    {
        canJump = false;
        audioManager.PlaySFX(audioManager.jump);
        jumpingPower = 0;
    }
}
