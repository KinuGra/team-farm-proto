using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3f;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);

        bool isMoving = move.magnitude > 0.1f;

        // 移動
        transform.Translate(move.normalized * moveSpeed * Time.deltaTime, Space.World);

        // 向きを移動方向に
        if (isMoving)
        {
            transform.forward = move;
        }

        // 👇 これだけでOK
        animator.SetBool("isWalking", isMoving);
    }
}