using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeThing : MonoBehaviour
{
    public DistanceJoint2D distanceJoint;
    public LineRenderer lineRenderer;
    public GameObject hook;

    // Start is called before the first frame update
    void Start()
    {
        distanceJoint.enabled = true;
        lineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*lineRenderer.SetPosition(1, new Vector3(distanceJoint.connectedAnchor.x, distanceJoint.connectedAnchor.y, 0));
        lineRenderer.SetPosition(0, new Vector3(distanceJoint.anchor.x, distanceJoint.anchor.y, 0));*/

        if (distanceJoint != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hook.transform.position);
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }

    }

    public void Fall()
    {
        Destroy(distanceJoint);
    }
}
