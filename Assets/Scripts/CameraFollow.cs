using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float height = 1.5f;

    private void LateUpdate()
    {
        if (target == null) return;
        transform.position = new Vector3(target.position.x, height, -10f);
    }
}
