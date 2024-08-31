using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    [Range(1, 10)]
    public float smoothFactor;

    public Vector3 minValues, maxValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = FindObjectOfType<PlayerController>().transform;
        Follow();
        
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValue.y),
            -10);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
