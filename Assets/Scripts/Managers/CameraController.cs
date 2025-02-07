using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Transform target;

    [Header("Settings")]
    [SerializeField] Vector2 minMaxXY;
    
    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("The Camera dont have a target");
            return;
        }

        Vector3 targetPosition = target.position;
        targetPosition.z = -10;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -minMaxXY.x, minMaxXY.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -minMaxXY.y, minMaxXY.y);

        transform.position = targetPosition;
    }
}
