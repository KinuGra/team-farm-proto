using UnityEngine;

public class preCameraFollow : MonoBehaviour
{
    public Transform target;        // �v���C���[
    public Vector3 offset = new Vector3(0, 8, -6);  // �΂ߏォ��̌����낵
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target);
    }
}