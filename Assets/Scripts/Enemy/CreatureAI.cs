using UnityEngine;

public class CreatureAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectRange = 20f;
    public float attackRange = 1.5f;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;
        if (GameOverManager.instance != null && GameOverManager.instance.IsGameOver()) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            transform.position += direction * moveSpeed * Time.deltaTime;

            if (distance <= attackRange)
            {
                GameOverManager.instance.GameOver();
            }
        }
    }
}