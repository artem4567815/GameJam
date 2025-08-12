using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = target.position;

            newPos.x = Mathf.Clamp(newPos.x, minPosition.x, maxPosition.x);
            newPos.y = Mathf.Clamp(newPos.y, minPosition.y, maxPosition.y);

            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }

}
