using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // ÉvÉåÉCÉÑÅ[
    public Vector3 offset = new Vector3(0, 8, -6);  // éŒÇﬂè„Ç©ÇÁÇÃå©â∫ÇÎÇµ
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target);
    }
}