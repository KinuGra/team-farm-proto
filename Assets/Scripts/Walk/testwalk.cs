using UnityEngine;

public class PlayerMove3D : MonoBehaviour
{
    public float speed = 5f;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, 0, z);

        // ★ここがポイント：方向キーの入力量を Speed に渡す
        anim.SetFloat("Speed", dir.magnitude);

        // 実際の移動
        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
