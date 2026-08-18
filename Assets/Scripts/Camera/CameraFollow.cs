using System.Runtime;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 1.0f;
    public float height = 1.5f;
    public float smoothTime = 0.1f;

    public float lookDistance = 1.0f;

    public float lookHeight = 0.5f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position -target.forward*distance + Vector3.up*height;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        Vector3 lookTarget = target.position + target.forward*lookDistance + Vector3.up*lookHeight;
        transform.LookAt(lookTarget);
    }
}