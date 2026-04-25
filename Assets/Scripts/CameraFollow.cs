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
    private float scale;
    void Start(){
        scale = target.localScale.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position -target.forward*scale*distance + Vector3.up*scale*height;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        Vector3 lookTarget = target.position + target.forward*scale*lookDistance + Vector3.up*scale*lookHeight;
        transform.LookAt(lookTarget);
    }
}