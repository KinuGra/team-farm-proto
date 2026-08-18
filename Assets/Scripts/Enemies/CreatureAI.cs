using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectRange = 20f;
    public float attackRange = 1.5f;
    public float rotationSpeed = 5f;

    private Transform player;
    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;
        if (GameOverManager.instance != null && GameOverManager.instance.IsGameOver())
        {
            if (animator != null) animator.SetBool("isWalking", false);
            return;
        }

        Vector3 myPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
        float distance = Vector3.Distance(myPos, playerPos);

        if (distance <= attackRange)
        {
            if (animator != null) animator.SetBool("isWalking", false);
            GameOverManager.instance.GameOver();
        }
        else if (distance <= detectRange)
        {
            Vector3 direction = (playerPos - myPos).normalized;

            // 滑らかに回転
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

            // MovePosition で移動（Kinematic でも動く）
            Vector3 newPos = transform.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            if (animator != null) animator.SetBool("isWalking", true);
        }
        else
        {
            if (animator != null) animator.SetBool("isWalking", false);
        }
    }
}