using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 9.0f;
    public float angularVelocity = 3.0f;
    float speed;

    float x, z;
    bool isRunning = false;
    
    private Rigidbody rb;
    private Transform tf;
    private Animator animator;

    [SerializeField] private ItemData onigiriItem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // 現状釣りのみ、キー入力を後でまとめる。
        if (Input.GetKeyDown(KeyCode.F))
        {   
            if(FishingManager.instance != null)
                FishingManager.instance.TryStartFishing();
        }

        // テスト用: Pキーでおにぎり取得
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (onigiriItem != null)
            {
                InventoryManager.instance.AddItem(onigiriItem);
            }
        }
    }
    void FixedUpdate()
    {
        // 移動制御, マウス無し移動ならこれが限界
        speed = isRunning ? runSpeed : walkSpeed;
        z = z>0 ? z : 0;
        Vector3 move =tf.forward * z;
        move = move.normalized;
        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);



        Vector3 rotation = tf.right * x + tf.forward * z;
        if (rotation.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(rotation);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rot,
                angularVelocity * Time.deltaTime
            );
        }

        // アニメーション切り替え
        bool isMoving = rotation.magnitude > 0.1f;
        if (animator != null)
        {
            if (isMoving)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Run", isRunning);
            }
            else
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
            }
        }
    }
    
}