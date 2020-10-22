using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    const float k_GroundedRadius = .2f;

    public float Speed;
    public float RunIncrement;
    public float JumpForce;
    public float Distance;
    public float GroundCheckRadius;

    public LayerMask whatIsLadder;
    public LayerMask whatIsGround;

    public UnityEngine.Transform groundCheck;

    private float inputHorizontal;
    private float inputVertical;   
        
    private bool isClimbling;
    private bool isGrounded;



    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    

    void Update() {
        CheckSurroundings();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (isGrounded == true && Input.GetButton("Run")){
            rb.velocity = new Vector2(inputHorizontal * Speed * RunIncrement, rb.velocity.y);
        } else{
            rb.velocity = new Vector2(inputHorizontal * Speed, rb.velocity.y);
        }


        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 2f, whatIsGround);              

        if (isGrounded && !isClimbling && Input.GetButton("Jump"))
        {
            rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
        

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, Distance, whatIsLadder);

        if (hitInfo.collider != null)
        {
            if (Input.GetButton("Vertical"))
            {
                isClimbling = true;
            }
        } else{
            isClimbling = false;
        }

        if(isClimbling == true)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * Speed);
            rb.gravityScale = 0;
        } else{
            rb.gravityScale = 5;
        }

        Debug.DrawRay(transform.position, Vector2.down * 2f, Color.red, 1);
        Debug.Log(rb.velocity.x);
        Debug.Log("grounded: " + isGrounded);
        //Debug.Log("climbling: "+isClimbling);
        Debug.Log(rb.velocity.y);
    }

    private void CheckSurroundings() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, whatIsGround);
    }
    
}
