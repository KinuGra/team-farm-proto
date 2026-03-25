using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z);

        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);

        // アニメーション切り替え
        bool isMoving = move.magnitude > 0.1f;
        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
        }

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }
}