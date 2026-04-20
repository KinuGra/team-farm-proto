using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 9.0f;
    float speed;

    float x, z;
    bool isRunning = false;
    
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }
    void FixedUpdate()
    {
        Vector3 move = new Vector3(x, 0, z);

        speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);

        // アニメーション切り替え
        bool isMoving = move.magnitude > 0.1f;
        if (animator != null)
        {
            if (isMoving)
            {
                animator.SetBool("isWalking", !isRunning);
                animator.SetBool("isRunning", isRunning);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }
}