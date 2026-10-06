using Unity.Properties;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;

    [Header("References")] 
    private Rigidbody2D rb;
    private Animator animator;
    
    private Vector2 movement;
    private bool isRunning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Prevent diagonal movement from being faster
        movement = movement.normalized;
        
        // Hold Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        UpdateAnimation();
        FlipCharacter();
    }

    private void FixedUpdate()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimation()
    {
        float speed = movement.magnitude;
        
        // 0 = Idle
        // 1 = Walking
        // 2 = Running
        if (speed == 0)
        {
            animator.SetInteger("MovementState", 0);
        }
        else if (isRunning)
        {
            animator.SetInteger("MovementState", 2);
        }
        else
        {
            animator.SetInteger("MovementState", 1);
        }
    }

    private void FlipCharacter()
    {
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
